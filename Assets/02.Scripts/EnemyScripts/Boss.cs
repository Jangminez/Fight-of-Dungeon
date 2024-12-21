using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Boss : Enemy
{
    public bool _isPattern;
    public bool _isStand;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        InitMonster();
    }

    public override void InitMonster()
    {
        if (!IsServer)
            return;

        _isPattern = false;

        MaxHp = 10000f;
        Hp = MaxHp;

        stat.attack = 800f;
        stat.attackRange = 5f;
        stat.attackSpeed = 0.5f;

        stat.defense = 1000f;

        stat.speed = 1.2f;
        stat.chaseRange = 10f;

        stat.isDie = false;

        anim.SetFloat("NormalState", 0f);

        StartCoroutine("MonsterState");
    }

    public override IEnumerator MonsterState()
    {
        if (!IsServer)
            yield break;

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

                if (
                    _target != null
                    && Vector2.Distance(_target.position, transform.position) < stat.chaseRange
                    && !stat.isDie
                    && !_isPattern
                )
                {
                    timer = 0f;
                    state = States.Chase;
                }
            }
            else if (state == States.Chase)
            {
                if (_target == null)
                    state = States.Idle;

                // 타겟의 위치 확인 후 이동
                Movement();
                SetDirection();

                if (
                    Vector2.Distance(_target.position, transform.position) < stat.attackRange && !stat.isDie)
                {
                    state = States.Attack;
                }
                else if (
                    Vector2.Distance(_target.position, transform.position) > stat.chaseRange
                    && !stat.isDie
                )
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

                if (
                    _target != null
                    && Vector2.Distance(_target.position, transform.position) > stat.attackRange
                    && !stat.isDie
                )
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

    public override void Die()
    {
        if (!IsServer)
            return;

        state = States.Die;
        StopAllCoroutines();
    }

    public override IEnumerator EnemyAttack()
    {
        if (_isPattern)
            yield break;

        // 공격시 방향 전환 및 애니메이션 실행
        SetDirection();
        anim.SetTrigger("Attack");

        // 공격속도 지연
        yield return new WaitForSeconds(1 / stat.attackSpeed);

        if (state != States.Attack)
        {
            _isAttack = false;
            _target = null;
            yield break;
        }
    }

    public override IEnumerator HitEffect()
    {
        yield break;
    }

    public override void Movement_Anim()
    {
        if (!IsServer)
            return;

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
