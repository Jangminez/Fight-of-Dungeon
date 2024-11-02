using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Pumkin : Enemy
{
    public GameObject _batPrefab;
    public GameObject _attackIndicator;
    Transform _attackFill;
    public GameObject _attackEffect;
    bool _indiOn = false;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        InitMonster();
    }

    // 몬스터 초기화
    public override void InitMonster()
    {
        if (!IsServer) return;

        if (!stat.isDie)
            _initTransform = this.transform.position;

        else
        {
            _isAttack = false;
            transform.position = _initTransform;
            RespawnClientRpc();
            state = States.Idle;
            StartCoroutine("HitEffect");
            anim.SetTrigger("Respawn");
        }

        MaxHp = 3500f;
        Hp = MaxHp;

        stat.attack = 500f;
        stat.attackRange = 4f;
        stat.attackSpeed = 0.5f;

        stat.defense = 300f;

        stat.speed = 1.3f;
        stat.chaseRange = 6f;

        stat.exp = 2500f;
        stat.gold = 3000;

        stat.isDie = false;
        _attackFill = _attackIndicator.transform.GetChild(0);

        StartCoroutine("MonsterState");
    }

    #region 피격 및 사망 처리
    public override void Hit(float damage)
    {
        //anim.SetTrigger("Hit");
        TakeDamageServerRpc(damage);
    }

    public override IEnumerator HitEffect()
    {
        SPUM_SpriteList spumList = transform.GetChild(0).GetComponent<SPUM_SpriteList>();
        List<SpriteRenderer> itemList = spumList._itemList;
        List<SpriteRenderer> armorList = spumList._armorList;
        List<SpriteRenderer> bodyList = spumList._bodyList;

        // 캐릭터의 Hair 색은 변경하지않음
        var filterItemList = itemList.Skip(2).ToList();

        foreach (var item in filterItemList)
        {
            item.color = Color.gray;
        }

        foreach (var armor in armorList)
        {
            armor.color = Color.gray;
        }

        foreach (var body in bodyList)
        {
            body.color = Color.gray;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var item in filterItemList)
        {
            item.color = Color.white;
        }

        foreach (var armor in armorList)
        {
            armor.color = Color.white;
        }

        foreach (var body in bodyList)
        {
            body.color = Color.white;
        }
    }

    public override void Die()
    {
        if (!IsServer) return;

        Hp = 0f;
        stat.isDie = true;

        state = States.Die;

        Invoke("InitMonster", 10f);

        OffAttackIndicatorClientRpc();

        int random_count = Random.Range(1, 4);

        for(int i = 0; i < random_count; i++){
            GameObject Bat = Instantiate(_batPrefab, transform.position, Quaternion.identity);

            if(!Bat.transform.GetChild(0).GetComponent<NetworkObject>().IsSpawned)
                Bat.transform.GetChild(0).GetComponent<NetworkObject>().Spawn(true);
        }
    }
    #endregion

    // 이동 애니메이션
    public override void Movement_Anim()
    {
        if (!IsServer) return;

        if (state == States.Chase || state == States.Return)
        {
            anim.SetFloat("RunState", 0.5f);
        }

        else
        {
            anim.SetFloat("RunState", 0f);
        }
    }

    public override IEnumerator EnemyAttack()
    {
        if (!IsServer) yield break;

        while (_isAttack)
        {
            

            while (anim.GetCurrentAnimatorStateInfo(2).IsName("2_Attack_Normal_pum"))
            {
                yield return null;
            }

            if (!_indiOn)
            {
                SetDirection();
                _indiOn = true;
                _attackFill.localScale = Vector3.zero;
                StartCoroutine(SetIndicator(_target));
            }

            yield return new WaitForSeconds(1 / stat.attackSpeed);

            if (state != States.Attack)
            {
                _isAttack = false;
                _target = null;
                yield break;
            }
        }
    }

    public override IEnumerator MonsterState()
    {
        if (!IsServer) yield break;

        while (!stat.isDie)
        {
            yield return null;

            if (_target == null && state != States.Idle && state != States.Return)
            {
                state = States.Idle;
            }

            if (state == States.Idle)
            {
                timer += Time.deltaTime;

                rb.velocity = Vector2.zero;

                if (_target != null && Vector2.Distance(_target.position, transform.position) < stat.chaseRange && !stat.isDie)
                {
                    timer = 0f;
                    state = States.Chase;
                }

                if (timer > 5f)
                {
                    timer = 0f;
                    state = States.Return;

                    Hp = MaxHp;
                }
            }

            else if (state == States.Chase)
            {
                if (_target == null)
                    state = States.Idle;

                // 타겟의 위치 확인 후 이동
                Movement();
                SetDirection();

                if (Vector2.Distance(_target.position, transform.position) < stat.attackRange && !stat.isDie)
                {
                    state = States.Attack;
                }

                else if (Vector2.Distance(_target.position, transform.position) > stat.chaseRange && !stat.isDie)
                {
                    state = States.Idle;
                    timer = 0f;
                }

            }

            else if (state == States.Attack)
            {
                rb.velocity = Vector2.zero;

                if (_target == null)
                {
                    state = States.Idle;
                    timer = 0f;
                }

                if (_target != null && Vector2.Distance(_target.position, transform.position) > stat.attackRange && !_isAttack && !stat.isDie)
                {
                    state = States.Chase;
                }
            }

            // 초기위치로 돌아감
            else if (state == States.Return)
            {
                Vector2 dirVec = _initTransform - this.transform.position;
                Vector2 nextVec = dirVec.normalized * stat.speed * Time.fixedDeltaTime;

                rb.MovePosition(rb.position + nextVec);

                if (Vector3.Distance(_initTransform, this.transform.position) < 0.1f)
                {
                    state = States.Idle;
                    timer = 0f;
                }
            }
        }
    }


    private IEnumerator SetIndicator(Transform target)
    {
        // 인디케이터 방향 설정
        if (target == null) yield break;

        Vector3 direction = (target.position - _attackIndicator.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _attackIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        

        OnAttackIndicatorClientRpc();

        StartCoroutine(FillIndicator(angle, direction));
    }

    private IEnumerator FillIndicator(float angle, Vector3 direction)
    {
        // 인디케이터 게이지 채우기
        float elapsedTime = 0f;
        float duration = 1 / stat.attackSpeed;

        while (elapsedTime < duration)
        {
            _attackFill.localScale = Vector3.Lerp(new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f), elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        OffAttackIndicatorClientRpc();
        GameObject slash = Instantiate(_attackEffect, transform.position, Quaternion.identity);
        slash.transform.rotation = Quaternion.Euler(new Vector3(0,0, angle));

        slash.GetComponent<Rigidbody2D>().velocity = direction * 3f;
        Destroy(slash, 1f);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(1f);
        _indiOn = false;
    }

    [ClientRpc]
    private void OnAttackIndicatorClientRpc()
    {
        _attackIndicator.SetActive(true);
    }

    [ClientRpc]
    private void OffAttackIndicatorClientRpc()
    {
        _attackIndicator.SetActive(false);
    }
}
