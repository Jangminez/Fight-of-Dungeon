using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Warrior : Player
{
    void Awake()
    {
        _playerRig = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        SetCharater();
    }

    protected override void SetCharater()
    {
        Die = false;

        MaxHp = 50.0f;
        Hp = MaxHp;
        HpRegen = 1f;

        MaxMp = 20.0f;
        Mp = MaxMp;
        MpRegen = 0.2f;

        Speed = 3.0f;

        Gold = 0;

        Attack = 6f;
        AttackSpeed = 1.0f;
        Critical = 0.0f;
        AttackRange = 2f;

        Defense = 5.0f;

        Level = 1;
        Exp = 0;
        NextExp = 100;

        StartCoroutine("Regen");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // �÷��̾� �̵�
        Movement();
    }

    private void LateUpdate()
    {
        // �÷��̾� �̵� �ִϸ��̼�
        Movement_Anim(); 
    }
}
