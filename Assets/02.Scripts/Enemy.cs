using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private SpriteRenderer spr;
    private Rigidbody2D rb;
    private Animator anim;
    public Vector3 _initTransform;
    public enum States { Idle, Chase, Attack, Die, Return}
    public States state;

    public Rigidbody2D _target;

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
        // �ʿ��� ���� ������Ʈ �Ҵ�
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();    
        _target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(state == States.Idle && !stat.isDie)
        {
            rb.velocity = Vector2.zero;

            if (Vector2.Distance(_target.position, rb.position) < 5f && !stat.isDie)
            {
                state = States.Chase;
            }
        }

        else if(state == States.Chase && !stat.isDie)
        {
            // �÷��̾� ����
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

        else if(state == States.Attack && !stat.isDie)
        {
            anim.SetTrigger("Attack");

            if (Vector2.Distance(_target.position, rb.position) > 2f && !stat.isDie)
            {
                state = States.Chase;
            }
        }

        else if(state == States.Die)
        {
            anim.SetTrigger("Die");
        }

        else if(state == States.Return)
        {
            Vector3 dirVec = _initTransform - this.transform.position;
            Vector3 nextVec = dirVec.normalized * stat.speed * Time.fixedDeltaTime;
     
            rb.MovePosition(rb.position + new Vector2(nextVec.x, nextVec.y));

            if(Vector3.Distance(_initTransform, this.transform.position) < 0.01f)
            {
                state = States.Idle;
            }
        }
    }
    // ���� �ʱ�ȭ �Լ�
    public abstract void InitMonster();


    #region ���� �ǰ� �� ���
    public virtual void Hit(float damage)
    {
        // ���� ������ ���
        float finalDamage = damage - stat.defense;
        if (finalDamage < 0f)
        {
            finalDamage = 0f;
        }

        Hp -= finalDamage;

        // �ǰ� ����Ʈ ����
        StartCoroutine("HitEffect");
        anim.SetTrigger("Hit");

        if (stat.hp <= 0)
        {
            StopAllCoroutines();

            Die();
        }
    }

    IEnumerator HitEffect()
    {
        spr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spr.color = Color.white;
    }

    public virtual void Die()
    {
        state = States.Die;

        Hp = 0f;
        stat.isDie = true;

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        spr.color = Color.gray;


        GiveExpGold(GameManager.Instance.player);

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

}
