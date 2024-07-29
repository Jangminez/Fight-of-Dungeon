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
    protected float _maxHp;
    protected float _hp;
    protected float _hpGeneration;
    protected float _maxMana;
    protected float _mana;
    protected float _manaGeneration;
    protected bool _isDie;
    protected int _gold;

    // 플레이어 공격관련 변수
    protected float _attack;
    protected float _attackSpeed;
    protected float _critical;
    protected float _defense;

    // 각 직업 초기화 함수
    abstract protected void SetCharater();

    #region 플레이어 스탯
    public float MaxHp
    {
        set => _maxHp = Mathf.Max(0, value);
        get => _maxHp;
    }

    public float Hp
    {
        set
        {
            if (_hp >= 0 && _hp <= _maxHp)
            {
                _hp = value;
            }
        }
        get => _hp;
    }

    public float HpGeneration
    {
        set => _hpGeneration = Mathf.Max(0, value);
        get => _hpGeneration;
    }

    public float MaxMana
    {
        set => _maxMana = Mathf.Max(0, value);
        get => _maxMana;
    }

    public float Mana
    {
        set
        {
            if (_mana >= 0 && _mana <= _maxMana)
            {
                _mana = value;
            }
        }
        get => _mana;
    }

    public float ManaGeneration
    {
        set => Mathf.Max(0, value);
        get => _manaGeneration;
    }

    public float Gold
    {
        set => _gold = (int)Mathf.Max(0, value);
        get => _gold;
    }

    public float Attack
    {
        set => Mathf.Max(0, value);
        get => _attack;
    }

    public float AttackSpeed
    {
        set => Mathf.Max(0, value);
        get => _attackSpeed;
    }

    public float Critical
    {
        set => Mathf.Max(0, value);
        get => _critical;
    }

    public float Defense
    {
        set => Mathf.Max(0,value);
        get => _defense;
    }


    #endregion

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
    abstract public void Hit();
    virtual protected void Die() 
    {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    #endregion
}
