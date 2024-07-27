using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Player : MonoBehaviour
{
    // 플레이어 이동
    protected Vector2 _inputVec;
    protected float _speed;
    protected Rigidbody2D _playerRig;

    // 플레이어 애니메이션
    protected Animator _animator;

    // 플레이어 상태 변수
    protected float _hp;
    protected float _mana;
    protected int _gold;
    protected bool _isDie;

    // 플레이어 공격관련 변수
    protected float _attack;
    protected float _attackSpeed;
    protected float _defense;

    #region 플레이어 이동 & 이동 애니메이션

    // InputSystem 값 받아오기
    void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();
    }

    // 플레이어 이동 구현
    public virtual void Movement()
    {
        Vector2 nextVec = _inputVec * _speed * Time.fixedDeltaTime;
        _playerRig.MovePosition(_playerRig.position + nextVec);
    }

    // 플레이어 이동 관련 애니메이션 구현
    public virtual void Movement_Anim()
    {
        if (_inputVec.x < 0)
        {
            _animator.SetInteger("Direction", 3);
        }
        else if (_inputVec.x > 0)
        {
            _animator.SetInteger("Direction", 2);
        }

        if (_inputVec.y > 0)
        {
            _animator.SetInteger("Direction", 1);
        }
        else if (_inputVec.y < 0)
        {
            _animator.SetInteger("Direction", 0);
        }
    }
    #endregion

    #region 플레이어 공격 & 공격 애니메이션
    abstract protected void BasicAttack();

    #endregion

    #region 플레이어 데미지 처리 & 사망
    abstract protected void Hit();
    virtual protected void Die() 
    {
        
    }

    #endregion
}
