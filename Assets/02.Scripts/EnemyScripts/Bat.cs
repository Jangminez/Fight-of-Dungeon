using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Bat : Enemy, IDamgeable
{

    public override void OnNetworkSpawn()
    {
        spr = GetComponent<SpriteRenderer>();

        if (!IsServer) return;

        InitMonster();
    }

    public override void InitMonster()
    {
        if (!IsServer) return;

        if (!stat.isDie)
            _initTransform = this.transform.position;

        MaxHp = 800f;
        Hp = MaxHp;

        stat.attack = 300f;
        stat.attackRange = 2f;
        stat.attackSpeed = 2f;

        stat.defense = 100f;

        stat.chaseRange = 5f;
        stat.speed = 2f;

        stat.exp = 1200f;
        stat.gold = 1000;

        stat.isDie = false;

        RespawnClientRpc();
        StartCoroutine("MonsterState");
    }

    public override IEnumerator EnemyAttack()
    {
        if (!IsServer) yield break;

        while (_isAttack)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1 / stat.attackSpeed);

            if (state != States.Attack)
            {
                _isAttack = false;
                _target = null;
                yield break;
            }

            if (_target != null && Vector2.Distance(_target.position , transform.position) < stat.attackRange)
                AttackClientRpc(_target.GetComponent<NetworkObject>().OwnerClientId, stat.attack);
        }
    }
    public void Hit(float damage)
    {
        StopCoroutine("EnemyAttack");
        _isAttack = false;
        anim.SetTrigger("Hit");
        TakeDamageServerRpc(damage);
    }

    public override IEnumerator HitEffect()
    {
        yield return null;
    }

    public override void Die()
    {
        Hp = 0f;
        stat.isDie = true;

        // 몬스터 상태 Die로 설정 후 애니메이션 실행
        state = States.Die;
        anim.SetFloat("RunState", 0f);

        StopAllCoroutines();
        //NetworkObjectPool.Instance.ReturnNetworkObject(GetComponent<NetworkObject>(), prefab);
    }

    public override void Movement_Anim()
    {
        if (state == States.Chase || state == States.Return)
        {
            anim.SetFloat("RunState", 0.5f);
        }

        else
        {
            anim.SetFloat("RunState", 0f);
        }
    }

}


