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

        stat.maxHp = 30f;
        Hp = stat.maxHp;

        stat.attack = 5f;
        stat.attackSpeed = 1.5f;

        stat.defense = 1f;

        stat.speed = 1f;

        stat.exp = 10f;
        stat.gold = 30;

        stat.isDie = false;

        StartCoroutine("MonsterState");
    }
}
