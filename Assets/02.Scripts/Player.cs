using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Player : MonoBehaviour
{
    // �÷��̾� �̵�
    protected Vector2 _inputVec;
    protected Rigidbody2D _playerRig;

    // �÷��̾� �ִϸ��̼�
    protected Animator _animator;

    // �÷��̾� ���� ����
    [Header("Player Stats")]
    [SerializeField] private float _maxHp;
    [SerializeField] private float _hp;
    [SerializeField] private float _hpGeneration;
    [Space(10f)]
    [SerializeField] private float _maxMana;
    [SerializeField] private float _mana;
    [SerializeField] private float _manaGeneration;
    [Space(10f)]
    [SerializeField] private float _attack;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _critical;
    [Space(10f)]
    [SerializeField] private float _defense;
    [Space(10f)]
    [SerializeField] private float _speed;

    // �÷��̾� ����
    protected bool _isDie;

    // �÷��̾� ����ġ & ���
    protected int _level;
    protected int _exp;
    protected int _gold;

    public Transform _target;

    // �� ���� �ʱ�ȭ �Լ�
    abstract protected void SetCharater();

    #region �÷��̾� ����
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
        set => _manaGeneration =Mathf.Max(0, value);
        get => _manaGeneration;
    }

    public float Attack
    {
        set => _attack = Mathf.Max(0, value);
        get => _attack;
    }

    public float AttackSpeed
    {
        set => _attackSpeed = Mathf.Max(0, value);
        get => _attackSpeed;
    }

    public float Critical
    {
        set => _critical =Mathf.Max(0, value);
        get => _critical;
    }

    public float Defense
    {
        set => _defense = Mathf.Max(0,value);
        get => _defense;
    }

    public int Gold
    {
        set => _gold = Mathf.Max(0, value);
        get => _gold;
    }

    public int Exp
    {
        set => _exp = Mathf.Max(0, value);
        get => _exp;
    }

    public int Level
    {
        set => _level = Mathf.Max(0, value);
        get => _level;
    }

    public float Speed
    {
        set => _speed = Mathf.Max(0, value);
        get => _speed;
    }

    #endregion

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
    [ContextMenu("Hit")]
    virtual public void Hit()
    {
        StartCoroutine(HitEffect());
    }

    virtual protected void Die() 
    {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
    }


    IEnumerator HitEffect()
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    #endregion
}
