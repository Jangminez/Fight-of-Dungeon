using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pumkin : Enemy
{
    public GameObject _attackIndicator;
    Transform _attackFill;
    public GameObject _attackEffect;
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
            SetDirection();

            while (anim.GetCurrentAnimatorStateInfo(2).IsName("2_Attack_Normal_pum"))
            {
                yield return null;
            }

            _attackFill.localScale = Vector3.zero;
            StartCoroutine(SetIndicator(_target));
            yield return new WaitForSeconds(1 / stat.attackSpeed);

            _attackIndicator.SetActive(false);
            anim.SetTrigger("Attack");

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
        if (target == null) yield break;

        Vector3 direction = (target.position - _attackIndicator.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _attackIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        _attackIndicator.SetActive(true);

        float elapsedTime = 0f;
        float duration = 1 / stat.attackSpeed;

        while(_attackIndicator.activeSelf)
        {
            _attackFill.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}
