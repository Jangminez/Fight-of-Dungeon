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
        _speed = 3.0f;
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

    protected override void Hit()
    {
        
    }
}
