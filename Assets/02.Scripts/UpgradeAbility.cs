using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAbility : MonoBehaviour
{
    [Header("Upgrade Button")]
    [SerializeField] Button _attackBtn;
    [SerializeField] Button _attackSpeedBtn;
    [SerializeField] Button _criticalBtn;
    [Space(10f)]
    [SerializeField] Button _maxHpBtn;
    [SerializeField] Button _hpGenerationBtn;
    [Space(10f)]
    [SerializeField] Button _defenseBtn;
    [Space(10f)]
    [SerializeField] Button _maxMpBtn;
    [SerializeField] Button _mpGenerationBtn;

    [Header("Upgrade Text")]
    [SerializeField] Text _attackLevel;
    [SerializeField] Text _attackValue;
    [SerializeField] Text _attackCost;
    [Space(10f)]
    [SerializeField] Text _asLevel;
    [SerializeField] Text _asValue;
    [SerializeField] Text _asCost;
    [Space(10f)]
    [SerializeField] Text _criticalLevel;
    [SerializeField] Text _criticalValue;
    [SerializeField] Text _criticalCost;
    [Space(10f)]
    [SerializeField] Text _maxHpLevel;
    [SerializeField] Text _maxHpValue;
    [SerializeField] Text _maxHpCost;
    [Space(10f)]
    [SerializeField] Text _hpRegenLevel;
    [SerializeField] Text _hpRegenValue;
    [SerializeField] Text _hpRegenCost;
    [Space(10f)]
    [SerializeField] Text _defenseLevel;
    [SerializeField] Text _defenseValue;
    [SerializeField] Text _defenseCost;
    [Space(10f)]
    [SerializeField] Text _maxMpLevel;
    [SerializeField] Text _maxMpValue;
    [SerializeField] Text _maxMpCost;
    [Space(10f)]
    [SerializeField] Text _mpRegenLevel;
    [SerializeField] Text _mpRegenValue;
    [SerializeField] Text _mpRegenCost;

    private int _attackLv , _attackSpeedLv, _criticalLv,
        _maxHpLv, _hpRegenLv,
        _defenseLv,
        _maxMpLv, _mpRegenLv = 1;


    private Player _player;

    private void Awake()
    {
        // 플레이어 찾기
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        // 각 버튼 이벤트 연결
        _attackBtn.onClick.AddListener(UpAttack);
        _attackSpeedBtn.onClick.AddListener(UpAttackSpeed);
        _criticalBtn.onClick.AddListener(UpCritical);
        _maxHpBtn.onClick.AddListener(UpMaxHp);
        _hpGenerationBtn.onClick.AddListener(UpHpGeneration);
        _defenseBtn.onClick.AddListener(UpDefense);
        _maxMpBtn.onClick.AddListener(UpMaxMp);
        _mpGenerationBtn.onClick.AddListener(UpMpGeneration);
    }

    private void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        // UI 초기화
        UpgradeValue(_attackLevel, _attackValue, _attackCost, _attackLv, _player.Attack, 5.0f, 0, "공격력");
        UpgradeValue(_asLevel, _asValue, _asCost, _attackSpeedLv, _player.AttackSpeed, 0.1f, 0, "공격속도");
        UpgradeValue(_criticalLevel, _criticalValue, _criticalCost, _criticalLv, _player.Critical, 2.0f, 0, "크리티컬 확률");
        UpgradeValue(_maxHpLevel, _maxHpValue, _maxHpCost, _maxHpLv, _player.MaxHp, 10f, 0, "체력");
        UpgradeValue(_hpRegenLevel, _hpRegenValue, _hpRegenCost, _hpRegenLv, _player.HpGeneration, 2.0f, 0, "체력 재생속도");
        UpgradeValue(_defenseLevel, _defenseValue, _defenseCost, _defenseLv, _player.Defense, 5.0f, 0, "방어력");
        UpgradeValue(_maxMpLevel, _maxMpValue, _maxMpCost, _maxMpLv, _player.MaxMana, 3.0f, 0, "마나");
        UpgradeValue(_mpRegenLevel, _mpRegenValue, _mpRegenCost, _mpRegenLv, _player.ManaGeneration, 1.0f, 0, "마나 재생속도");
    }

    // 공격 업그레이드
    private void UpAttack()
    {
        Debug.Log("공격 업!");

        _player.Attack += 5f;
        _attackLv += 1;
        UpgradeValue(_attackLevel, _attackValue, _attackCost, _attackLv ,_player.Attack, 5.0f, 5, "공격력");

    }

    // 공격속도 업그레이드
    private void UpAttackSpeed()
    {
        Debug.Log("공격속도 업!");

        _player.AttackSpeed += 0.1f;
        _player.AttackSpeed = (float)Math.Round(_player.AttackSpeed, 1);
        _attackSpeedLv += 1;
        UpgradeValue(_asLevel, _asValue, _asCost, _attackSpeedLv, _player.AttackSpeed, 0.1f, 10, "공격속도");
    }

    // 크리티컬 확률 업그레이드
    private void UpCritical()
    {
        Debug.Log("크리티컬 확률 업!");

        _player.Critical += 2f;
        _criticalLv += 1;
        UpgradeValue(_criticalLevel, _criticalValue, _criticalCost, _criticalLv, _player.Critical, 2.0f, 100, "크리티컬 확률");
    }

    // 체력 업그레이드
    private void UpMaxHp()
    {
        Debug.Log("최대 체력 업!");

        _player.MaxHp += 10f;
        _maxHpLv += 1;
        UpgradeValue(_maxHpLevel, _maxHpValue, _maxHpCost, _maxHpLv, _player.MaxHp, 10f, 10, "체력");
    }

    // 체력 재생속도 업그레이드
    private void UpHpGeneration()
    {
        Debug.Log("체력 재생속도 업!");

        _player.HpGeneration += 2f;
        _hpRegenLv += 1;
        UpgradeValue(_hpRegenLevel, _hpRegenValue, _hpRegenCost, _hpRegenLv, _player.HpGeneration, 2f, 50, "체력 재생속도");
    }

    // 방어력 업그레이드
    private void UpDefense()
    {
        Debug.Log("방어력 업!");

        _player.Defense += 5f;
        _defenseLv += 1;
        UpgradeValue(_defenseLevel, _defenseValue, _defenseCost, _defenseLv, _player.Defense, 5f, 20, "방어력");
    }

    // 마나 업그레이드
    private void UpMaxMp()
    {
        Debug.Log("최대 마나 업!");

        _player.MaxMana += 5f;
        _maxMpLv += 1;
        UpgradeValue(_maxMpLevel, _maxMpValue, _maxMpCost, _maxMpLv, _player.MaxMana, 3f, 50, "마나");
    }

    // 마나 재생속도 업그레이드
    private void UpMpGeneration()
    {
        Debug.Log("마나 재생속도 업!");

        _player.ManaGeneration += 1f;
        _mpRegenLv += 1;
        UpgradeValue(_mpRegenLevel, _mpRegenValue, _mpRegenCost, _mpRegenLv, _player.ManaGeneration, 1f, 100, "마나 재생속도");
    }


    private void UpgradeValue(Text level, Text value, Text cost, int Lv ,float initValue ,float increase, int costInc, string name)
    {
        //UI 변경
        if (name == "크리티컬 확률")
        {
            level.text = "Lv" + (Lv + 1).ToString() + " " + name;
            value.text = initValue.ToString() + "%" + " -> " + (initValue + increase).ToString() + "%";
            cost.text = (Int32.Parse(cost.text) + costInc).ToString();
            return;
        }

        else if(name == "체력 재생속도" || name =="마나 재생속도")
        {
            level.text = "Lv" + (Lv + 1).ToString() + " " + name;
            value.text = "초당 " + initValue.ToString() + " -> " + (initValue + increase).ToString();
            cost.text = (Int32.Parse(cost.text) + costInc).ToString();
            return;
        }

        level.text = "Lv" + (Lv + 1 ).ToString() + " " + name;
        value.text = Math.Round(initValue, 1).ToString() + " -> " + Math.Round(initValue + increase,1).ToString();
        cost.text = (Int32.Parse(cost.text) + costInc).ToString();
    }
}
