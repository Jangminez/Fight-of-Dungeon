using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using System.Xml.Schema;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Jobs;

public abstract class Player : MonoBehaviour
{
    protected Vector2 _inputVec;
    protected Rigidbody2D _playerRig;

    protected Animator _animator;

    [Header("Player Stats")]
    [SerializeField] private float _maxHp;
    [SerializeField] private float _hp;
    [SerializeField] private float _hpBonus;
    [SerializeField] private float _hpRegen;
    [SerializeField] private float _hpRegenBonus;

    [Space(10f)]
    [SerializeField] private float _maxMp;
    [SerializeField] private float _mp;
    [SerializeField] private float _mpBonus;
    [SerializeField] private float _mpRegen;
    [SerializeField] private float _mpRegenBonus;
    
    [Space(10f)]
    [SerializeField] private float _attack;
    [SerializeField] private float _attackBonus;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _asBonus;
    [SerializeField] private float _finalAs;
    [SerializeField] private float _critical;
    [Space(10f)]
    [SerializeField] private float _defense;
    [SerializeField] private float _defenseBonus;

    [Space(10f)]
    [SerializeField] private float _speed;

    [Space(10f)]
    [SerializeField] private float _attackRange;

    [Space(10f)]
    [SerializeField] private bool _isDie;

    [Space(10f)]
    [SerializeField] private int _level;

    [SerializeField] private float _exp;
    [SerializeField] private float _nextExp;
    [SerializeField] private int _lvPoint;

    [Space(10f)]
    [SerializeField] private int _gold;

    [Space(10f)]
    public Transform _target;
    [Space(10f)]
    public Transform _spawnPoint;
    // 플레이어 초기화 함수
    abstract protected void SetCharater();

    #region 플레이어 스탯
    public float MaxHp
    {
        set
        {
            if (_maxHp != value)
            {
                _maxHp = Mathf.Max(0, value);
                GetComponent<PlayerUIController>().HpChanged();
            }

        }
        get => _maxHp;
    }

    public float Hp
    {
        set
        {
            if (_hp >= 0 && _hp <= FinalHp && _hp != value)
            {
                _hp = Mathf.Max(0, value);
                GetComponent<PlayerUIController>().HpChanged();
            }

        }
        get => _hp;
    }

    public float HpBonus   
    {
        set => _hpBonus = Mathf.Max(0, value);
        get => _hpBonus;
    }

    public float HpRegen
    {
        set => _hpRegen = Mathf.Max(0, value);
        get => _hpRegen;
    }

    public float HpRegenBonus
    {
        set => _hpRegenBonus = Mathf.Max(0, value);
        get => _hpRegenBonus;
    }

    public float MaxMp
    {
        set
        {
            if (_maxMp != value)
            {
                _maxMp = Mathf.Max(0, value);
                GetComponent<PlayerUIController>().MpChanged();
            }
        }
        get => _maxMp;
    }

    public float Mp
    {
        set
        {
            if (_mp >= 0 && _mp <= FinalMp && _mp != value)
            {
                _mp = Mathf.Max(0, value);
                GetComponent<PlayerUIController>().MpChanged();
            }
        }
        get => _mp;
    }

        public float MpBonus   
    {
        set => _mpBonus = Mathf.Max(0, value);
        get => _mpBonus;
    }

    public float MpRegen
    {
        set => _mpRegen =Mathf.Max(0, value);
        get => _mpRegen;
    }

        public float MpRegenBonus
    {
        set => _mpRegenBonus = Mathf.Max(0, value);
        get => _mpRegenBonus;
    }

    public float Attack
    {
        set => _attack = Mathf.Max(0, value);
        get => _attack;
    }

    public float AttackBonus
    {
        set => _attackBonus = Mathf.Max(0, value);
        get => _attackBonus;
    }

    public float AttackSpeed
    {
        set => _attackSpeed = Mathf.Max(0, value);
        get => _attackSpeed;
    }

    public float AsBonus
    {
        set => _asBonus = Mathf.Max(0, value);
        get => _asBonus;
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

        public float DefenseBonus
    {
        set => _defenseBonus = Mathf.Max(0, value);
        get => _defenseBonus;
    }

    public int Gold
    {
        set 
        {
            if (_gold != value)
            {
                _gold = Mathf.Max(0, value);
                GetComponent<PlayerUIController>().GoldChanged();
            }
        }
        get => _gold;
    }

    public float NextExp
    {
        set => _nextExp = Mathf.Round(Mathf.Max(0, value));
        get => _nextExp;
    }

    public float Exp
    {
        set
            {
                if (_exp != value)
                {
                    _exp = Mathf.Round(Mathf.Max(0, value));
                    GetComponent<PlayerUIController>().ExpChanged();

                    if(_exp >= _nextExp)
                    {
                        LevelUp();
                    }
                }
            }

        get => _exp;
    }

    public int LvPoint
    {
        set 
        {
            if(LvPoint != value)
            {
                _lvPoint = Mathf.Max(0, value);
                GetComponent<PlayerUIController>().LvPointChange();
            }
        }
        get => _lvPoint;
    }

    public int Level
    {
        set
        {
            if (_level != value)
            {
                _level = Mathf.Max(0, value);
                GetComponent<PlayerUIController>().LevelChanged();
            }
        }

        get => _level;
    }

    public float Speed
    {
        set => _speed = Mathf.Max(0, value);
        get => _speed;
    }

    public bool Die
    {
        protected set
        {
            _isDie = value;

            if (_isDie)
            {
                GetComponent<PlayerUIController>().OnRespawn();
            }
        }
        get => _isDie;
    }

    public float AttackRange
    {
        set => _attackRange = Mathf.Max(0, value);
        get => _attackRange;
    }

    #endregion

    #region 플레이어 최종 스탯
    public float FinalHp => _maxHp * (1 + (_hpBonus * 0.01f));
    public float FinalHpRegen => _hpRegen * (1 + (_hpRegenBonus * 0.01f));
    public float FinalMp => _maxMp * (1 + (_mpBonus * 0.01f));
    public float FinalMpRegen => _mpRegen * (1 + (_mpRegenBonus * 0.01f));
    public float FinalAttack => _attack * (1 + (_attackBonus * 0.01f));
    public float FinalAS => _attackSpeed * (1 + (_asBonus * 0.01f));
    public float FinalDefense => _defense * (1 + (_defenseBonus * 0.01f));
    #endregion

    #region 플레이어 이동 및 애니메이션

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

        SetDirection();
        if(nextVec ==  Vector2.zero)
        {
            _animator.SetFloat("RunState", 0f);
        }
    }

    // 플레이어 이동 애니메이션
    public virtual void Movement_Anim()
    {
        if(_inputVec.x !=0  || _inputVec.y !=0)
        {
            _animator.SetFloat("RunState", 0.5f);
        }

        else
        {
            _animator.SetFloat("RunState", 0f);
        }
    }

    void SetDirection()
    {
        if(_inputVec.x > 0)
        {
            _animator.transform.localScale = new Vector3(-1, 1, 1);
        }

        else if (_inputVec.x < 0)
        {
            _animator.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    #endregion

    #region 플레이어 이벤트 처리
    public void Hit(float damage)
    {
        if (Die)
            return;

        float finalDm = damage - FinalDefense;
        if(finalDm <= 0)
            finalDm = 1;

        Hp -= finalDm;

        if (Hp == 0f)
        {
            OnDie();
        }

        else
        {
            StartCoroutine(HitEffect());
        }
    }

    IEnumerator HitEffect()
    {
        //this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        //this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    [ContextMenu("Die")]
    protected void OnDie() 
    {
        Die = true;
        Hp = 0f;
        _target = null;

        _animator.SetTrigger("Die");
        // ���� X, �浹 X
        this.GetComponent<PlayerInput>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;
        this.GetComponent<PlayerFindTarget>().enabled = false;
        // ĳ���� ȿ��

        StopCoroutine("Regen");
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(10f);

        this.GetComponent<PlayerInput>().enabled = true;
        this.GetComponent<Collider2D>().enabled = true;
        this.GetComponent<PlayerFindTarget>().enabled = true;


        _isDie = false;
        Hp = FinalHp;
        Mp = FinalMp;

        StartCoroutine("Regen");

        this.transform.position = _spawnPoint.transform.position + new Vector3(0f, 1f, 0f);
        _animator.SetTrigger("Respawn");
    }

    virtual protected void LevelUp()
    {
        _exp -= _nextExp;
        NextExp *= 1.5f;

        Level += 1;
        LvPoint += 5;

        GetComponent<PlayerUIController>().ExpChanged();

        if (_exp >= _nextExp)
        {
            LevelUp();
        }
    }

    IEnumerator Regen()
    {
        if (!_isDie)
        {
            if (Hp < FinalHp)
            {
                Hp += FinalHpRegen;

                if (Hp >= FinalHp)
                    Hp = FinalHp;
            }

            if (Mp < FinalMp)
            {
                Mp += FinalMpRegen;

                if (Mp >= FinalMp)
                    Mp = FinalMp;
            }
        }

        yield return new WaitForSeconds(1);

        StartCoroutine("Regen");
    }
    #endregion

    #region 테스트용 함수
        private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            GetGold();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GetExp();
        }
    }

    [ContextMenu("Get Gold")]
    virtual public void GetGold()
    {
        Gold += 500000;
    }

    [ContextMenu("Get Exp")]
    virtual public void GetExp()
    {
        Exp += 1000;
    }
    #endregion
}
 