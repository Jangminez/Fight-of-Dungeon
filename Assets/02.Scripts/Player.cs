using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Player : MonoBehaviour
{
    // �÷��̾� �̵�
    protected Vector2 _inputVec;
    protected float _speed;
    protected Rigidbody2D _playerRig;

    // �÷��̾� �ִϸ��̼�
    protected Animator _animator;

    // �÷��̾� ���� ����
    protected float _hp;
    protected float _mana;
    protected int _gold;
    protected bool _isDie;

    // �÷��̾� ���ݰ��� ����
    protected float _attack;
    protected float _attackSpeed;
    protected float _defense;

    #region �÷��̾� �̵� & �̵� �ִϸ��̼�

    // InputSystem �� �޾ƿ���
    void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();
    }

    // �÷��̾� �̵� ����
    public virtual void Movement()
    {
        Vector2 nextVec = _inputVec * _speed * Time.fixedDeltaTime;
        _playerRig.MovePosition(_playerRig.position + nextVec);
    }

    // �÷��̾� �̵� ���� �ִϸ��̼� ����
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

    #region �÷��̾� ���� & ���� �ִϸ��̼�
    abstract protected void BasicAttack();

    #endregion

    #region �÷��̾� ������ ó�� & ���
    abstract protected void Hit();
    virtual protected void Die() 
    {
        
    }

    #endregion
}
