using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private SpriteRenderer spr;
    private Rigidbody2D rb;

    public Rigidbody2D _target;

    [Serializable]
    public struct Stats
    {
        public float hp;
        public float attack;
        public float attackSpeed;
        public float defense;
        public float speed;
        public float exp;
        public int gold;
        public bool isDie;
    }

    public Stats stats;

    private void Awake()
    {
        // �ʿ��� ���� ������Ʈ �Ҵ�
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // �÷��̾� ����
        if (Vector2.Distance(_target.position, rb.position) < 5f && !stats.isDie)
        {
            Vector2 dirVec = _target.position - rb.position;
            Vector2 nextVec = dirVec.normalized * stats.speed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + nextVec);
        }
    }
    // ���� �ʱ�ȭ �Լ�
    public abstract void InitMonster();


    #region ���� �ǰ� �� ���
    public virtual void Hit(float damage)
    {
        // ���� ������ ���
        float finalDamage = damage - stats.defense;
        if (finalDamage < 0f)
        {
            finalDamage = 0f;
        }

        stats.hp -= finalDamage;

        // �ǰ� ����Ʈ ����
        StartCoroutine("HitEffect");

        if (stats.hp <= 0)
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
        stats.hp = 0f;
        stats.isDie = true;
        GetComponent<Collider2D>().enabled = false;
        spr.color = Color.gray;

        GiveExpGold(GameManager.Instance.player);
    }

    public virtual void GiveExpGold(Player player)
    {
        player.Exp += stats.exp;
        player.Gold += stats.gold;
    }

    #endregion
}
