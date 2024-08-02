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
        MaxHp = 100.0f;
        Hp = MaxHp;
        HpGeneration = 1f;
        MaxMana = 50.0f;
        Mana = MaxMana;
        ManaGeneration = 0.2f;
        Speed = 3.0f;
        Gold = 0;
        Attack = 10.0f;
        AttackSpeed = 1.0f;
        Critical = 0.0f;
        Defense = 10.0f;
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

    protected override void BasicAttack()
    {

    }
}
