using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected SpriteRenderer spr;
    protected Rigidbody2D rb;
    protected Animator anim;
    public Vector3 _initTransform;
    public enum States { Idle, Chase, Attack, Return, Die}
    public States state;

    public Rigidbody2D _target;
    protected bool _isAttack;

    public GameObject FloatingDamagePrefab;
    public GameObject FloatingGoldExpPrefab;
    private float timer;

    [Serializable]
    public struct Stats
    {
        public float maxHp;
        public float hp;
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

    public float MaxHp
    {
        set => stat.maxHp = Mathf.Max(0, value);
        get => stat.maxHp;
    }

    public float Hp
    {
        set
        {
            if (stat.hp >= 0 && stat.hp != value)
            {
                stat.hp = Mathf.Max(0, value);
                GetComponent<MonsterHp>().ChangeHp();
            }

        }
        get => stat.hp;
    }



    private void Awake()
    {
        //spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();    
        _target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        Movement_Anim();
        
        // 공격 여부 판정
        if(!_isAttack && state == States.Attack)
        {
            _isAttack = true;
            anim.SetFloat("AttackSpeed", stat.attackSpeed);
            StartCoroutine("EnemyAttack");
        }    
    }

    IEnumerator MonsterState()
    {
        while (!stat.isDie)
        {
            yield return null;

            if (state == States.Idle)
            {
                timer += Time.deltaTime;

                rb.velocity = Vector2.zero;

                if (Vector2.Distance(_target.position, rb.position) < stat.chaseRange && !stat.isDie)
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
                // 타겟의 위치 확인 후 이동
                Movement();
                SetDirection();

                if (Vector2.Distance(_target.position, rb.position) < stat.attackRange && !stat.isDie)
                {
                    state = States.Attack;
                }

                else if (Vector2.Distance(_target.position, rb.position) > stat.chaseRange && !stat.isDie)
                {
                    state = States.Idle;
                }

            }

            else if (state == States.Attack)
            {
                if (Vector2.Distance(_target.position, rb.position) > stat.attackRange && !stat.isDie)
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
                }
            }

            if (GameManager.Instance.player.Die)
            {
                state = States.Return;
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
        Vector2 dirVec = _target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * stat.speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nextVec);
    }
    public abstract void Movement_Anim();

    public abstract IEnumerator EnemyAttack();

    public virtual void GiveExpGold(Player player)
    {
        player.Exp += stat.exp;
        player.Gold += stat.gold;
        ShowGoldExp();
    }

        void SetDirection()
    {
        if(_target.position.x - rb.position.x > 0)
        {
            anim.transform.localScale = new Vector3(-1f, 1f, 1);
        }

        else if (_target.position.x - rb.position.x < 0)
        {
            anim.transform.localScale = new Vector3(1f, 1f, 1);
        }
    }
    virtual public void OutofArea()
    {
        state = States.Return;
    }

    public virtual void ShowFloatingDamage(float damage) 
    {
        var dmg = Instantiate(FloatingDamagePrefab, transform.position, Quaternion.identity);
        dmg.GetComponent<TextMesh>().text = $"-{damage}";
    }

    public virtual void ShowGoldExp()
    {
        var floating = Instantiate(FloatingGoldExpPrefab, transform.position, Quaternion.identity);
        floating.GetComponent<TextMesh>().text = $"+{stat.exp}Exp\n+{stat.gold}Gold";
    }
}


