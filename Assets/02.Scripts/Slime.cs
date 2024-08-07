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
        if(!stat.isDie)
            _initTransform = this.transform.position;

        stat.maxHp = 100f;
        stat.hp = stat.maxHp;

        stat.attack = 10f;
        stat.attackSpeed = 0.5f;

        stat.defense = 5f;

        stat.speed = 1f;

        stat.exp = 10f;
        stat.gold = 30;

        stat.isDie = false;
    }
}
