using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private void Start()
    {
        InitMonster();
    }

    public override void InitMonster()
    {
        if (!stat.isDie)
            _initTransform = this.transform.position;

        else
        {
            transform.position = _initTransform;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = true;
            spr.color = Color.white;
            state = States.Idle;
            anim.SetTrigger("Respawn");
        }

        stat.maxHp = 100f;
        Hp = stat.maxHp;

        stat.attack = 10f;
        stat.attackSpeed = 0.5f;

        stat.defense = 5f;

        stat.speed = 1f;

        stat.exp = 10f;
        stat.gold = 30;

        stat.isDie = false;

        StartCoroutine("MonsterState");
    }
}
