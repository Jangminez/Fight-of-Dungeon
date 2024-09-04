using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected SpriteRenderer spr;
    private Rigidbody2D rb;
    protected Animator anim;
    public Vector3 _initTransform;
    public enum States { Idle, Chase, Attack, Return, Die}
    public States state;

    public Rigidbody2D _target;

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

    IEnumerator MonsterState()
    {
        while (!stat.isDie)
        {
            yield return null;

            if (state == States.Idle)
            {
                timer += Time.deltaTime;

                rb.velocity = Vector2.zero;

                if (Vector2.Distance(_target.position, rb.position) < 5f && !stat.isDie)
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
                Vector2 dirVec = _target.position - rb.position;
                Vector2 nextVec = dirVec.normalized * stat.speed * Time.fixedDeltaTime;

                rb.MovePosition(rb.position + nextVec);

                if (Vector2.Distance(_target.position, rb.position) < 2f && !stat.isDie)
                {
                    state = States.Attack;
                }

                else if (Vector2.Distance(_target.position, rb.position) > 5f && !stat.isDie)
                {
                    state = States.Idle;
                }

            }

            else if (state == States.Attack)
            {
                anim.SetTrigger("Attack");

                if (Vector2.Distance(_target.position, rb.position) > 2f && !stat.isDie)
                {
                    state = States.Chase;
                }
            }

            else if (state == States.Return)
            {
                Vector3 dirVec = _initTransform - this.transform.position;
                Vector3 nextVec = dirVec.normalized * stat.speed * Time.fixedDeltaTime;

                rb.MovePosition(rb.position + new Vector2(nextVec.x, nextVec.y));

                if (Vector3.Distance(_initTransform, this.transform.position) < 0.01f)
                {
                    state = States.Idle;
                }
            }

            if (GameManager.Instance.player.Die)
            {
                state = States.Return;
            }
        }
        Die();

    }
    // 몬스터 초기화 함수
    public abstract void InitMonster();


    #region 몬스터 피격 및 사망 이벤트
    public virtual void Hit(float damage)
    {
        float finalDamage = damage - stat.defense;
        if (finalDamage < 0f)
        {
            finalDamage = 1f;
        }

        Hp -= finalDamage;

        if(FloatingDamagePrefab != null && stat.hp > 0){
            ShowFloatingDamage(finalDamage);
        }

        StartCoroutine("HitEffect");
        anim.SetTrigger("Hit");

        if (stat.hp <= 0)
        {
            StopAllCoroutines();

            anim.SetTrigger("Die");
            Die();
        }
    }

    IEnumerator HitEffect()
    {
        //spr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        //spr.color = Color.white;
    }

    public virtual void Die()
    {
        Hp = 0f;
        stat.isDie = true;

        state = States.Die;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        spr.color = Color.gray;


        GiveExpGold(GameManager.Instance.player);
        ShowGoldExp();

        Invoke("InitMonster", 10f);

    }

    public virtual void GiveExpGold(Player player)
    {
        player.Exp += stat.exp;
        player.Gold += stat.gold;
    }

    #endregion

    virtual public void OutofArea()
    {
        state = States.Return;
    }

    void ShowFloatingDamage(float damage) 
    {
        var dmg = Instantiate(FloatingDamagePrefab, transform.position, Quaternion.identity, transform);
        dmg.GetComponent<TextMesh>().text = $"-{damage.ToString()}";
    }

    void ShowGoldExp()
    {
        var floating = Instantiate(FloatingGoldExpPrefab, transform.position, Quaternion.identity, transform);
        floating.GetComponent<TextMesh>().text = $"+{stat.exp}Exp\n+{stat.gold}Gold";
    }
}
