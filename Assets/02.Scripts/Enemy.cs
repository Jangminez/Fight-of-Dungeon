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
        // 필요한 변수 컴포넌트 할당
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 플레이어 추적
        if (Vector2.Distance(_target.position, rb.position) < 5f && !stats.isDie)
        {
            Vector2 dirVec = _target.position - rb.position;
            Vector2 nextVec = dirVec.normalized * stats.speed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + nextVec);
        }
    }
    // 몬스터 초기화 함수
    public abstract void InitMonster();


    #region 몬스터 피격 및 사망
    public virtual void Hit(float damage)
    {
        // 최종 데미지 계산
        float finalDamage = damage - stats.defense;
        if (finalDamage < 0f)
        {
            finalDamage = 0f;
        }

        stats.hp -= finalDamage;

        // 피격 이펙트 실행
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
