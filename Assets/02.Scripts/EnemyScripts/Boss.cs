using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Boss : Enemy, IDamgeable
{
    public bool _isPattern;
    public bool _isStand;
    private float r_Pattern;

    [SerializeField]
    private ulong _lastAttackClientId;

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

        _initTransform = transform.position;

        _isPattern = false;

        MaxHp = 10000f;
        Hp = MaxHp;

        stat.attack = 1000f;
        stat.attackRange = 5f;
        stat.attackSpeed = 0.5f;

        stat.defense = 1000f;

        stat.speed = 1.2f;
        stat.chaseRange = 10f;

        stat.isDie = false;

        anim.SetFloat("RunState", 0f);

        state = States.Idle;
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

                if (_target != null && Vector2.Distance(_target.position, transform.position) < stat.chaseRange && !stat.isDie)
                {
                    timer = 0f;
                    state = States.Chase;
                }

                if (timer > 5f && Vector2.Distance(_initTransform, transform.position) >= 0.5f)
                {
                    state = States.Return;
                }
            }
            else if (state == States.Chase && _isStand)
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
                    Vector2.Distance(_target.position, transform.position) > stat.chaseRange && !stat.isDie)
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

                if (Vector3.Distance(_initTransform, this.transform.position) < 0.5f)
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
        GameManager.Instance.GameOver(_lastAttackClientId);
    }

    public override IEnumerator EnemyAttack()
    {
        if (!IsServer) yield break;

        yield return new WaitForSeconds(0.1f);
        // 공격시 방향 전환 및 애니메이션 실행
        SetDirection();

        r_Pattern = Random.Range(0f, 101f);

        if (r_Pattern <= 50f)
        {
            // 기본 공격
            _isPattern = true;
            StartCoroutine(Boss_BasicAttack());
        }

        else if (50f < r_Pattern && r_Pattern <= 75f)
        {
            // 점프공격
            _isPattern = true;
            StartCoroutine(Boss_JumpAttack());
        }

        else
        {
            // 회전공격
            _isPattern = true;
            StartCoroutine(Boss_SpinAttack());
        }

        yield return new WaitForSeconds(1 / stat.attackSpeed);

        if (state != States.Attack)
        {
            _isAttack = false;
            _target = null;
            yield break;
        }
    }

    public void Hit(float damage)
    {
        TakeDamageServerRpc(damage);
        SetLastAttackClientServerRpc();
    }

    public override IEnumerator HitEffect()
    {
        yield break;
    }

    public override void Movement_Anim()
    {
        if (!IsServer)
            return;


        if (state == States.Chase || state == States.Return || state == States.Attack)
        {
            anim.SetFloat("RunState", 1f);
        }
        else
        {
            anim.SetFloat("RunState", 0f);
        }
    }

    private IEnumerator Boss_BasicAttack()
    {
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(1f);

        _isAttack = false;
    }

    private IEnumerator Boss_JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        stat.speed = 0f;
        yield return new WaitForSeconds(1f);
        stat.attackRange = 0f;
        stat.speed = 5f;
        yield return new WaitForSeconds(1f);
        stat.speed = 0f;
        yield return new WaitForSeconds(2f);
        stat.attackRange = 5f;
        stat.speed = 1.2f;

        _isAttack = false;
    }

    private IEnumerator Boss_SpinAttack()
    {
        anim.SetTrigger("SpinAttack");

        yield return new WaitForSeconds(4f);

        _isAttack = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetLastAttackClientServerRpc(ServerRpcParams rpcParams = default)
    {
        // 마지막으로 공격한 클라이언트의 아이디 저장
        _lastAttackClientId = rpcParams.Receive.SenderClientId;
    }
}
