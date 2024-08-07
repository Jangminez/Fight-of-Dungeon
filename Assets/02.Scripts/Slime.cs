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
        stats.hp = 100f;
        stats.attack = 10f;
        stats.attackSpeed = 0.5f;
        stats.defense = 5f;
        stats.speed = 1f;

        stats.exp = 10f;
        stats.gold = 30;

        stats.isDie = false;
    }
}
