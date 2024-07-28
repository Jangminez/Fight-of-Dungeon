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
        _hp = 100.0f;
        _mana = 50.0f;
        _speed = 3.0f;
        _gold = 0;
        _attack = 10.0f;
        _attackSpeed = 1.0f;
        _defense = 10f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 플레이어 이동
        Movement();
    }

    private void LateUpdate()
    {
        // 플레이어 이동 애니메이션
        Movement_Anim();
    }

    protected override void BasicAttack()
    {

    }

    public override void Hit()
    {
        
    }
}
