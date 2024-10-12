using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public abstract class Enemy : NetworkBehaviour
{
    protected SpriteRenderer spr;
    protected Rigidbody2D rb;
    protected Animator anim;
    public Vector3 _initTransform;
    public enum States { Idle, Chase, Attack, Return, Die }
    public States state;

    public Transform _target;
    protected bool _isAttack;

    public GameObject FloatingDamagePrefab;
    public GameObject FloatingGoldExpPrefab;
    private float timer;

    [Serializable]
    public struct Stats
    {
        public float attack;
        public float attackSpeed;
        public float defense;
        public float speed;
        public float attackRange;
        public float chaseRange;
        public float exp;
        public int gold;
        public bool isDie;
    }

    public Stats stat;

    // 몬스터의 체력에 대한 NetworkVariable
    private NetworkVariable<float> _maxHp = new NetworkVariable<float>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<float> _hp = new NetworkVariable<float>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public float MaxHp
    {
        set => _maxHp.Value = Mathf.Max(0, value);
        get => _maxHp.Value;
    }

    public float Hp
    {
        set
        {
            if (_hp.Value >= 0 && _hp.Value != value)
            {
                _hp.Value = Mathf.Max(0, value);
            }

        }
        get => _hp.Value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _hp.OnValueChanged += GetComponent<EnemyHp>().ChangeHp;
    }

    void LateUpdate()
    {
        if (!IsServer) return;

        Movement_Anim();

        // 공격 여부 판정
        if (!_isAttack && state == States.Attack)
        {
            _isAttack = true;
            anim.SetFloat("AttackSpeed", stat.attackSpeed);
            StartCoroutine("EnemyAttack");
        }
    }

    IEnumerator MonsterState()
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
                if (_target == null)
                {
                    state = States.Idle;
                    timer = 0f;
                }

                if (_target != null && Vector2.Distance(_target.position, transform.position) > stat.attackRange && !stat.isDie)
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
    // 몬스터 초기화 함수
    public abstract void InitMonster();
    public abstract void Hit(float damage);

    public abstract IEnumerator HitEffect();
    public abstract void Die();

    public virtual void Movement()
    {
        if (!IsServer) return;

        if (_target == null)
        {
            state = States.Idle;
            timer = 0f;
            return;
        }

        Vector2 dirVec = _target.position - transform.position;
        Vector2 nextVec = dirVec.normalized * stat.speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nextVec);
    }
    public abstract void Movement_Anim();

    public abstract IEnumerator EnemyAttack();

    [ServerRpc(RequireOwnership = false)]
    public void GiveExpGoldServerRpc(ulong lastClientId)
    {
        ShowGoldClientRpc(lastClientId, stat.gold, stat.exp);
    }

    public virtual void SetDirection()
    {
        if (_target != null && _target.position.x - transform.position.x > 0)
        {
            anim.transform.localScale = new Vector3(-1f, 1f, 1);
        }

        else if (_target != null && _target.position.x - transform.position.x < 0)
        {
            anim.transform.localScale = new Vector3(1f, 1f, 1);
        }
    }
    virtual public void OutofArea()
    {
        state = States.Return;
    }
    
    [ClientRpc]
    protected void AttackClientRpc(ulong clientId, float damage)
    {
        // 공격 받은 클라이언트라면 Hit() 처리
        if(clientId == NetworkManager.Singleton.LocalClientId)
            GameManager.Instance.player.Hit(damage: damage);
    }

    [ClientRpc]
    public void ShowFloatingDamageClientRpc(float damage)
    {
        // 몬스터 피격 데미지 표시
        var dmg = Instantiate(FloatingDamagePrefab, transform.position, Quaternion.identity);
        dmg.GetComponent<TextMesh>().text = $"-" + damage.ToString("F1");
    }

    [ClientRpc]
    public virtual void ShowGoldClientRpc(ulong clientId, int gold, float exp)
    {
        // 마지막으로 처치한 클라이언트라면 골드와 경험치 지급
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            GameManager.Instance.player.Gold += gold;
            GameManager.Instance.player.Exp += exp;
            var floating = Instantiate(FloatingGoldExpPrefab, transform.position, Quaternion.identity);
            floating.GetComponent<TextMesh>().text = $"\n+{gold}Gold";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    protected void TakeDamageServerRpc(float damage, ServerRpcParams rpcParams = default)
    {
        // 받은 데미지 - 방어력 으로 최종데미지 계산
        float finalDamage = damage - stat.defense;

        if (finalDamage < 0f)
        {
            finalDamage = 1f;
        }

        Hp -= finalDamage;

        if (FloatingDamagePrefab != null && Hp > 0)
        {
            // 데미지 표시 동기화
            ShowFloatingDamageClientRpc(finalDamage);
        }

        anim.SetTrigger("Hit");

        if (Hp <= 0)
        {
            // 체력이 0 이하라면 Die
            StopAllCoroutines();

            anim.SetTrigger("Die");

            Die();
            DieClientRpc(rpcParams.Receive.SenderClientId);
        }
        else
        {
            // 죽는게 아니라면 HitEffect 실행
            StartCoroutine("HitEffect");
        }
    }

    [ClientRpc]
    protected void DieClientRpc(ulong lastAttackClient)
    {
        // 속도 0, 콜라이더 비활성화를 통한 플레이어의 공격 금지        
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // 경험치랑 골드 지급
        if (NetworkManager.Singleton.LocalClientId == lastAttackClient)
            GiveExpGoldServerRpc(lastAttackClient);
    }

    [ClientRpc]
    protected void RespawnClientRpc()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = true;
    }
}


