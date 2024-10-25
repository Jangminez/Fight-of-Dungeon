using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pumkin : Enemy
{
    public GameObject _attackIndicator;
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
        stat.attackRange = 5f;
        stat.attackSpeed = 0.5f;

        stat.defense = 300f;

        stat.speed = 1.3f;
        stat.chaseRange = 7f;

        stat.exp = 2500f;
        stat.gold = 3000;

        stat.isDie = false;

        _attackIndicator.transform.GetChild(0).GetComponent<Animator>().SetFloat("AttackSpeed", stat.attackSpeed);
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
            if (state != States.Attack)
            {
                _isAttack = false;
                _target = null;
                _attackIndicator.SetActive(false);
                anim.ResetTrigger("Attack");
                yield break;
            }

            SetDirection();
            SetIndicator(_target);
            yield return new WaitForSeconds(1 / stat.attackSpeed);
            _attackIndicator.SetActive(false);
            anim.SetTrigger("Attack");
        }
    }

    private void SetIndicator(Transform target)
    {
        if(target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _attackIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        _attackIndicator.SetActive(true);
    }
}
