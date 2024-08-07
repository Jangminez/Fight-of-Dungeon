using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAbility : MonoBehaviour
{
    [Serializable]
    public struct HUD // UI ǥ��
    {
        public Button btn;
        public Text level;
        public Text value;
        public Text cost;
    }

    [Serializable]
    public struct UpgradeInfo   // ���׷��̵� ���� ������, ���� ���, ����
    {
        public enum upgradeType { Attack, AttackSpeed, Critical, MaxHp, HpRegen, Defense ,MaxMp, MpRegen };
        public upgradeType type;
        public float incValue;
        public float incCost;
        public int level;
    }

    public HUD myUI;
    public UpgradeInfo upgradeInfo;

    private Player _player;

    private void Awake()
    {
        // �÷��̾� ã��
        _player = GameManager.Instance.player;

        // ��ư Ŭ���̺�Ʈ�� �Լ� ����
        myUI.btn.onClick.AddListener(Upgrade);

        // ���׷��̵� �׸� ���� �ʱ�ȭ
        upgradeInfo.level = 0;
    }

    private void Start()
    {
        // UI �ʱ�ȭ �Լ�
        InitUI();
    }

    private void InitUI()
    {
        // UI �ʱ�ȭ
        switch (upgradeInfo.type)
        {
            case UpgradeInfo.upgradeType.Attack:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Attack, upgradeInfo.incValue, 1, "���ݷ�");
                break;

            case UpgradeInfo.upgradeType.AttackSpeed:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.AttackSpeed, upgradeInfo.incValue, 1, "���ݼӵ�");
                break;

            case UpgradeInfo.upgradeType.Critical:
                _player.Critical += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Critical, upgradeInfo.incValue, 1, "ũ��Ƽ�� Ȯ��");
                break;

            case UpgradeInfo.upgradeType.MaxHp:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MaxHp, upgradeInfo.incValue, 1, "ü��");
                break;

            case UpgradeInfo.upgradeType.HpRegen:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.HpGeneration, upgradeInfo.incValue, 1, "ü�� ����ӵ�");
                break;

            case UpgradeInfo.upgradeType.Defense:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Defense, upgradeInfo.incValue, 1, "����");
                break;

            case UpgradeInfo.upgradeType.MaxMp:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MaxMp, upgradeInfo.incValue, 1, "����");
                break;

            case UpgradeInfo.upgradeType.MpRegen:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MpGeneration, upgradeInfo.incValue, 1, "���� ����ӵ�");
                break;
        }
    }

    private void Upgrade()
    {
        switch (upgradeInfo.type)
        {
            case UpgradeInfo.upgradeType.Attack:
                _player.Attack += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Attack, upgradeInfo.incValue, upgradeInfo.incCost, "���ݷ�");
                break;

            case UpgradeInfo.upgradeType.AttackSpeed:
                _player.AttackSpeed += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.AttackSpeed, upgradeInfo.incValue, upgradeInfo.incCost, "���ݼӵ�");
                break;

            case UpgradeInfo.upgradeType.Critical:
                _player.Critical += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Critical, upgradeInfo.incValue, upgradeInfo.incCost, "ũ��Ƽ�� Ȯ��");
                break;

            case UpgradeInfo.upgradeType.MaxHp:
                _player.MaxHp += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MaxHp, upgradeInfo.incValue, upgradeInfo.incCost, "ü��");
                break;

            case UpgradeInfo.upgradeType.HpRegen:
                _player.HpGeneration += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.HpGeneration, upgradeInfo.incValue, upgradeInfo.incCost, "ü�� ����ӵ�");
                break;

            case UpgradeInfo.upgradeType.Defense:
                _player.Defense += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Defense, upgradeInfo.incValue, upgradeInfo.incCost, "����");
                break;

            case UpgradeInfo.upgradeType.MaxMp:
                _player.MaxMp += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MaxMp, upgradeInfo.incValue, upgradeInfo.incCost, "����");
                break;

            case UpgradeInfo.upgradeType.MpRegen: 
                _player.MpGeneration += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MpGeneration, upgradeInfo.incValue, upgradeInfo.incCost, "���� ����ӵ�");
                break;
        }

        upgradeInfo.level += 1;
    }

    private void SetUI(Text level, Text value, Text cost, int Lv ,float initValue ,float increase, float costInc, string name)
    {
        //UI ����
        if (name == "ũ��Ƽ�� Ȯ��")
        {
            level.text = "Lv" + (Lv + 1).ToString() + " " + name;
            value.text = initValue.ToString() + "%" + " -> " + (initValue + increase).ToString() + "%";
            cost.text = Mathf.Round((float.Parse(cost.text) * costInc)).ToString();
            return;
        }

        else if(name == "ü�� ����ӵ�" || name =="���� ����ӵ�")
        {
            level.text = "Lv" + (Lv + 1).ToString() + " " + name;
            value.text = "�ʴ� " + Math.Round(initValue, 1).ToString() + " -> " + Math.Round((initValue + increase), 1).ToString();
            cost.text = Mathf.Round((float.Parse(cost.text) * costInc)).ToString();
            return;
        }

        level.text = "Lv" + (Lv + 1 ).ToString() + " " + name;
        value.text = Math.Round(initValue, 1).ToString() + " -> " + Math.Round(initValue + increase,1).ToString();
        cost.text = Mathf.Round((float.Parse(cost.text) * costInc)).ToString();
    }
}
