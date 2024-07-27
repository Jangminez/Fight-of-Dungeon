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

    protected override void Hit()
    {
        
    }
}
