using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public abstract class Player : NetworkBehaviour
{
    [SerializeField] protected Rigidbody2D _playerRig;

    [SerializeField] protected Animator _animator;
    #region 플레이어 스탯 변수
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

    #endregion
    // 플레이어 초기화 함수
    abstract protected void SetCharater();

    #region 플레이어 스탯 프로퍼티
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
            if (_hp != value)
            {
                _hp = Mathf.Max(0, value);
                GetComponent<PlayerUIController>().HpChanged();
            }

            if(value >= FinalHp)
            {
                _hp = FinalHp;
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

                if(_mp > _maxMp){
                    _mp = _maxMp;
                }

                GetComponent<PlayerUIController>().MpChanged();
            }
        }
        get => _maxMp;
    }

    public float Mp
    {
        set
        {
            if (_mp != value)
            {
                _mp = Mathf.Max(0, value);
                GetComponent<PlayerUIController>().MpChanged();
            }

            if (value > FinalMp)
            {
                _mp = FinalMp;
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
                UIManager.Instance.GoldChanged();
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
                    UIManager.Instance.ExpChanged();

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
                UIManager.Instance.LvPointChange();
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
                UIManager.Instance.LevelChanged();
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

            if (_isDie && IsOwner)
            {
                UIManager.Instance.OnRespawn();
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

    #region 플레이어 이벤트 처리
    public void Hit(float damage)
    {
        if(!IsOwner) return;

        if (Die)
            return;

        float finalDamage = damage - FinalDefense;
        if(finalDamage <= 0)
            finalDamage = 1;

        Hp -= finalDamage;

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
        if(!IsOwner) yield break;

        SPUM_SpriteList spumList = transform.GetChild(0).GetComponent<SPUM_SpriteList>();
        if(spumList == null)
            yield break;
        
        List<SpriteRenderer> itemList = spumList._itemList;
        List<SpriteRenderer> armorList = spumList._armorList;
        List<SpriteRenderer> bodyList = spumList._bodyList;

        // 캐릭터의 Hair 색은 변경하지않음
        var filterItemList = itemList.Skip(2).ToList();

        foreach(var item in filterItemList)
        {
            item.color = Color.red;
        }

        foreach(var armor in armorList)
        {
            armor.color = Color.red;
        }

        foreach(var body in bodyList)
        {
            body.color = Color.red;
        }

        yield return new WaitForSeconds(0.2f);

        foreach(var item in filterItemList)
        {
            item.color = Color.white;
        } 

        foreach(var armor in armorList)
        {
            armor.color = Color.white;
        }

        foreach(var body in bodyList)
        {
            body.color = Color.white;
        }
    }

    [ContextMenu("Die")]
    protected void OnDie() 
    {
        if(!IsOwner) return;

        Die = true;
        Hp = 0f;
        _target = null;
        _playerRig.velocity = Vector2.zero;

        _animator.SetTrigger("Die");
        // 사망시 이동 입력, 충돌, 공격 정지
        this.GetComponent<PlayerMovement>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;
        this.GetComponent<PlayerFindTarget>().enabled = false;

        // 체력, 마나 재생 정지 & 리스폰 기능 활성화
        StopCoroutine("Regen");
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn()
    {
        if(!IsOwner) yield break;

        yield return new WaitForSeconds(10f);

        this.GetComponent<PlayerMovement>().enabled = true;
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
        if(!IsOwner) return;

        _exp -= _nextExp;
        NextExp *= 1.5f;

        Level += 1;
        LvPoint += 5;

        UIManager.Instance.ExpChanged();

        if(Level == 5)
        {
            Destroy(UIManager.Instance.locked[0]);
        }

        else if(Level == 10)
        {
            Destroy(UIManager.Instance.locked[1]);
        } 
          
        if (_exp >= _nextExp)
        {
            LevelUp();
        }


    }

    IEnumerator Regen()
    {
        if(!IsOwner) yield break;

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
        if(!IsOwner) return;
        
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
 