using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
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

    // ���� �ʱ�ȭ �Լ�
    public abstract void InitMonster();

    public virtual void Hit(float damage)
    {
        // ���� ������ ���
        float finalDamage = damage - stats.defense;
        if(finalDamage < 0f)
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
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void Die()
    {
        stats.hp = 0f;
        stats.isDie = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = Color.gray;

        GiveExpGold(GameManager.Instance.player);
    }

    public virtual void GiveExpGold(Player player)
    {
        player.Exp += stats.exp;
        player.Gold += stats.gold;
    }
}
