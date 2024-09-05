using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goblin : Enemy
{
    void Start()
    {
        InitMonster();
    }
    public override void InitMonster()
    {
        if (!stat.isDie)
            _initTransform = this.transform.position;

        else
        {
            _isAttack = false;
            transform.position = _initTransform;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = true;
            state = States.Idle;
            StartCoroutine("HitEffect");
            anim.SetTrigger("Respawn");
        }

        stat.maxHp = 3500f;
        Hp = stat.maxHp;

        stat.attack = 500f;
        stat.attackRange = 8f;
        stat.attackSpeed = 1f;

        stat.defense = 500f;

        stat.speed = 1.3f;
        stat.chaseRange = 10f;

        stat.exp = 1000f;
        stat.gold = 1300;

        stat.isDie = false;

        // 원거리 애니메이션
        anim.SetFloat("NormalState", 0.5f);

        StartCoroutine("MonsterState");
    }

    public override void Hit(float damage)
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

        if (stat.hp <= 0)
        {
            StopAllCoroutines();

            anim.SetTrigger("Die");
            Die();
        }
    }

    public override IEnumerator HitEffect()
    {
        SPUM_SpriteList spumList = transform.GetChild(0).GetComponent<SPUM_SpriteList>();
        List<SpriteRenderer> itemList = spumList._itemList;
        List<SpriteRenderer> armorList = spumList._armorList;
        List<SpriteRenderer> bodyList = spumList._bodyList;

        // 캐릭터의 Hair 색은 변경하지않음
        var filterItemList = itemList.Skip(2).ToList();

        foreach(var item in filterItemList)
        {
            item.color = Color.gray;
        }

        foreach(var armor in armorList)
        {
            armor.color = Color.gray;
        }

        foreach(var body in bodyList)
        {
            body.color = Color.gray;
        }

        yield return new WaitForSeconds(0.2f);

        foreach(var item in filterItemList)
        {
            item.color = Color.white;
        } 

        foreach(var armor in armorList)
        {
            armor.color = Color.white;
        }

        foreach(var body in bodyList)
        {
            body.color = Color.white;
        }
    }

    public override void Die()
    {
        Hp = 0f;
        stat.isDie = true;

        state = States.Die;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        GiveExpGold(GameManager.Instance.player);
        ShowGoldExp();

        Invoke("InitMonster", 10f);
    }

    public override void Movement_Anim()
    {
        if(state == States.Chase)
        {
            anim.SetFloat("RunState", 0.5f);
        }

        else
        {
            anim.SetFloat("RunState", 0f);
        }
    }

    public override IEnumerator EnemyAttack()
    {

        while(_isAttack)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1 / stat.attackSpeed);

            if (state != States.Attack) 
            {
                _isAttack = false;
                yield break;
            }
            
            GameManager.Instance.player.Hit(damage: stat.attack);
        }
    }
}
