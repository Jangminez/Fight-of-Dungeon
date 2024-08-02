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
        // �÷��̾� ã��
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        // �� ��ư �̺�Ʈ ����
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
        // UI �ʱ�ȭ
        UpgradeValue(_attackLevel, _attackValue, _attackCost, _attackLv, _player.Attack, 5.0f, 0, "���ݷ�");
        UpgradeValue(_asLevel, _asValue, _asCost, _attackSpeedLv, _player.AttackSpeed, 0.1f, 0, "���ݼӵ�");
        UpgradeValue(_criticalLevel, _criticalValue, _criticalCost, _criticalLv, _player.Critical, 2.0f, 0, "ũ��Ƽ�� Ȯ��");
        UpgradeValue(_maxHpLevel, _maxHpValue, _maxHpCost, _maxHpLv, _player.MaxHp, 10f, 0, "ü��");
        UpgradeValue(_hpRegenLevel, _hpRegenValue, _hpRegenCost, _hpRegenLv, _player.HpGeneration, 2.0f, 0, "ü�� ����ӵ�");
        UpgradeValue(_defenseLevel, _defenseValue, _defenseCost, _defenseLv, _player.Defense, 5.0f, 0, "����");
        UpgradeValue(_maxMpLevel, _maxMpValue, _maxMpCost, _maxMpLv, _player.MaxMana, 3.0f, 0, "����");
        UpgradeValue(_mpRegenLevel, _mpRegenValue, _mpRegenCost, _mpRegenLv, _player.ManaGeneration, 1.0f, 0, "���� ����ӵ�");
    }

    // ���� ���׷��̵�
    private void UpAttack()
    {
        Debug.Log("���� ��!");

        _player.Attack += 5f;
        _attackLv += 1;
        UpgradeValue(_attackLevel, _attackValue, _attackCost, _attackLv ,_player.Attack, 5.0f, 5, "���ݷ�");

    }

    // ���ݼӵ� ���׷��̵�
    private void UpAttackSpeed()
    {
        Debug.Log("���ݼӵ� ��!");

        _player.AttackSpeed += 0.1f;
        _player.AttackSpeed = (float)Math.Round(_player.AttackSpeed, 1);
        _attackSpeedLv += 1;
        UpgradeValue(_asLevel, _asValue, _asCost, _attackSpeedLv, _player.AttackSpeed, 0.1f, 10, "���ݼӵ�");
    }

    // ũ��Ƽ�� Ȯ�� ���׷��̵�
    private void UpCritical()
    {
        Debug.Log("ũ��Ƽ�� Ȯ�� ��!");

        _player.Critical += 2f;
        _criticalLv += 1;
        UpgradeValue(_criticalLevel, _criticalValue, _criticalCost, _criticalLv, _player.Critical, 2.0f, 100, "ũ��Ƽ�� Ȯ��");
    }

    // ü�� ���׷��̵�
    private void UpMaxHp()
    {
        Debug.Log("�ִ� ü�� ��!");

        _player.MaxHp += 10f;
        _maxHpLv += 1;
        UpgradeValue(_maxHpLevel, _maxHpValue, _maxHpCost, _maxHpLv, _player.MaxHp, 10f, 10, "ü��");
    }

    // ü�� ����ӵ� ���׷��̵�
    private void UpHpGeneration()
    {
        Debug.Log("ü�� ����ӵ� ��!");

        _player.HpGeneration += 2f;
        _hpRegenLv += 1;
        UpgradeValue(_hpRegenLevel, _hpRegenValue, _hpRegenCost, _hpRegenLv, _player.HpGeneration, 2f, 50, "ü�� ����ӵ�");
    }

    // ���� ���׷��̵�
    private void UpDefense()
    {
        Debug.Log("���� ��!");

        _player.Defense += 5f;
        _defenseLv += 1;
        UpgradeValue(_defenseLevel, _defenseValue, _defenseCost, _defenseLv, _player.Defense, 5f, 20, "����");
    }

    // ���� ���׷��̵�
    private void UpMaxMp()
    {
        Debug.Log("�ִ� ���� ��!");

        _player.MaxMana += 5f;
        _maxMpLv += 1;
        UpgradeValue(_maxMpLevel, _maxMpValue, _maxMpCost, _maxMpLv, _player.MaxMana, 3f, 50, "����");
    }

    // ���� ����ӵ� ���׷��̵�
    private void UpMpGeneration()
    {
        Debug.Log("���� ����ӵ� ��!");

        _player.ManaGeneration += 1f;
        _mpRegenLv += 1;
        UpgradeValue(_mpRegenLevel, _mpRegenValue, _mpRegenCost, _mpRegenLv, _player.ManaGeneration, 1f, 100, "���� ����ӵ�");
    }


    private void UpgradeValue(Text level, Text value, Text cost, int Lv ,float initValue ,float increase, int costInc, string name)
    {
        //UI ����
        if (name == "ũ��Ƽ�� Ȯ��")
        {
            level.text = "Lv" + (Lv + 1).ToString() + " " + name;
            value.text = initValue.ToString() + "%" + " -> " + (initValue + increase).ToString() + "%";
            cost.text = (Int32.Parse(cost.text) + costInc).ToString();
            return;
        }

        else if(name == "ü�� ����ӵ�" || name =="���� ����ӵ�")
        {
            level.text = "Lv" + (Lv + 1).ToString() + " " + name;
            value.text = "�ʴ� " + initValue.ToString() + " -> " + (initValue + increase).ToString();
            cost.text = (Int32.Parse(cost.text) + costInc).ToString();
            return;
        }

        level.text = "Lv" + (Lv + 1 ).ToString() + " " + name;
        value.text = Math.Round(initValue, 1).ToString() + " -> " + Math.Round(initValue + increase,1).ToString();
        cost.text = (Int32.Parse(cost.text) + costInc).ToString();
    }
}
