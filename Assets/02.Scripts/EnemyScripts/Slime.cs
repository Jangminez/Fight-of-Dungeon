using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Slime : Enemy
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

        else
        {
            anim.SetTrigger("Respawn");
            _isAttack = false;
            RespawnClientRpc();
            state = States.Idle;
        }

        MaxHp = 30f;
        Hp = MaxHp;

        stat.attack = 5f;
        stat.attackRange = 2f;
        stat.attackSpeed = 1.5f;

        stat.defense = 1f;

        stat.chaseRange = 5f;
        stat.speed = 1f;

        stat.exp = 10f;
        stat.gold = 30;

        stat.isDie = false;

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
    public override void Hit(float damage)
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
        anim.ResetTrigger("Hit");
        anim.SetFloat("RunState", 0f);
        StopAllCoroutines();
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

