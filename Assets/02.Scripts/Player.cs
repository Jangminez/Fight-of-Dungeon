using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Player : MonoBehaviour
{
    // 플레이어 이동
    protected Vector2 _inputVec;
    protected Rigidbody2D _playerRig;

    // 플레이어 애니메이션
    protected Animator _animator;

    // 플레이어 스탯 변수
    [Header("Player Stats")]
    [SerializeField] private float _maxHp;
    [SerializeField] private float _hp;
    [SerializeField] private float _hpGeneration;
    [Space(10f)]
    [SerializeField] private float _maxMp;
    [SerializeField] private float _mp;
    [SerializeField] private float _mpGeneration;
    [Space(10f)]
    [SerializeField] private float _attack;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _critical;
    [Space(10f)]
    [SerializeField] private float _defense;
    [Space(10f)]
    [SerializeField] private float _speed;

    // 플레이어 상태
    protected bool _isDie;

    // 플레이어 경험치 & 골드
    protected int _level;
    protected int _exp;
    protected int _gold;

    public Transform _target;

    public Transform _spawnPoint;

    // 각 직업 초기화 함수
    abstract protected void SetCharater();

    IEnumerator Regen()
    {
        if(Hp < MaxHp)
        {
            Hp += HpGeneration;

            if(Hp >= MaxHp)
                Hp = MaxHp;
        }

        if(Mp < MaxMp)
        {
            Mp += MpGeneration;

            if(Mp >= MaxMp)
                Mp = MaxMp;
        }


        yield return new WaitForSeconds(1);

        StartCoroutine("Regen");
    }

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
                _hp = Mathf.Max(0, value);
            }

        }
        get => _hp;
    }

    public float HpGeneration
    {
        set => _hpGeneration = Mathf.Max(0, value);
        get => _hpGeneration;
    }

    public float MaxMp
    {
        set => _maxMp = Mathf.Max(0, value);
        get => _maxMp;
    }

    public float Mp
    {
        set
        {
            if (_mp >= 0 && _mp <= _maxMp)
            {
                _mp = Mathf.Max(0, value);
            }
        }
        get => _mp;
    }

    public float MpGeneration
    {
        set => _mpGeneration =Mathf.Max(0, value);
        get => _mpGeneration;
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
    abstract protected void Skill1();
    abstract protected void Skill2();
    abstract protected void Skill3();

    #endregion

    #region 플레이어 데미지 처리 & 사망 & 리스폰
    [ContextMenu("Hit")]
    public void Hit()
    {
        //Hp -= damage;

        Hp -= 30f;

        if (Hp == 0f)
        {
            Die();
        }

        else
        {
            StartCoroutine(HitEffect());
        }
    }

    IEnumerator HitEffect()
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    [ContextMenu("Die")]
    protected void Die() 
    {
        _isDie = true;
        Hp = 0f;

        this.GetComponent<PlayerInput>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<SpriteRenderer>().color = Color.gray;

        StopCoroutine("Regen");
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(10f);

        this.GetComponent<PlayerInput>().enabled = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
        this.GetComponent<SpriteRenderer>().color = Color.white;

        _isDie = false;
        Hp = MaxHp;
        Mp = MaxMp;

        StartCoroutine("Regen");

        this.transform.position = _spawnPoint.transform.position + new Vector3(0f, 1f, 0f);
    }
    #endregion
}
