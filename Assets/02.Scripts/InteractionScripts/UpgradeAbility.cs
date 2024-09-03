using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAbility : MonoBehaviour
{
    [Serializable]
    public struct HUD // UI 변수
    {
        public Button btn;
        public Text level;
        public Text value;
        public Text cost;
    }

    [Serializable]
    public struct UpgradeInfo   // 업그레이드 정보 
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
        // 플레이어 찾기
        _player = GameManager.Instance.player;

        // 버튼 클릭시 실행할 함수 연결
        myUI.btn.onClick.AddListener(Upgrade);

        // 업그레이드 레벨
        upgradeInfo.level = 0;
    }

    private void Start()
    {
        // UI 초기화
        InitUI();
    }

    private void InitUI()
    {
        // UI 초기화
        switch (upgradeInfo.type)
        {
            case UpgradeInfo.upgradeType.Attack:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Attack, upgradeInfo.incValue, 1, "공격력");
                break;

            case UpgradeInfo.upgradeType.AttackSpeed:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.AttackSpeed, upgradeInfo.incValue, 1, "공격속도");
                break;

            case UpgradeInfo.upgradeType.Critical:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Critical, upgradeInfo.incValue, 1, "크리티컬 확률");
                break;

            case UpgradeInfo.upgradeType.MaxHp:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MaxHp, upgradeInfo.incValue, 1, "체력");
                break;

            case UpgradeInfo.upgradeType.HpRegen:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.HpRegen, upgradeInfo.incValue, 1, "체력 재생속도");
                break;

            case UpgradeInfo.upgradeType.Defense:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Defense, upgradeInfo.incValue, 1, "방어력");
                break;

            case UpgradeInfo.upgradeType.MaxMp:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MaxMp, upgradeInfo.incValue, 1, "마나");
                break;

            case UpgradeInfo.upgradeType.MpRegen:
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MpRegen, upgradeInfo.incValue, 1, "마나 재생속도");
                break;
        }
    }

    private void Upgrade()
    {
        // 골드가 충분하면 업그레이드 진행
        if (_player.Gold >= Int32.Parse(myUI.cost.text))
        {
            _player.Gold -= Int32.Parse(myUI.cost.text);
        }

        else return;

        switch (upgradeInfo.type)
        {
            case UpgradeInfo.upgradeType.Attack:
                _player.Attack += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Attack, upgradeInfo.incValue, upgradeInfo.incCost, "공격력");
                break;

            case UpgradeInfo.upgradeType.AttackSpeed:
                _player.AttackSpeed += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.AttackSpeed, upgradeInfo.incValue, upgradeInfo.incCost, "공격속도");
                break;

            case UpgradeInfo.upgradeType.Critical:
                _player.Critical += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Critical, upgradeInfo.incValue, upgradeInfo.incCost, "크리티컬 확률");
                break;

            case UpgradeInfo.upgradeType.MaxHp:
                _player.MaxHp += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MaxHp, upgradeInfo.incValue, upgradeInfo.incCost, "체력");
                break;

            case UpgradeInfo.upgradeType.HpRegen:
                _player.HpRegen += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.HpRegen, upgradeInfo.incValue, upgradeInfo.incCost, "체력 재생속도");
                break;

            case UpgradeInfo.upgradeType.Defense:
                _player.Defense += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.Defense, upgradeInfo.incValue, upgradeInfo.incCost, "방어력");
                break;

            case UpgradeInfo.upgradeType.MaxMp:
                _player.MaxMp += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MaxMp, upgradeInfo.incValue, upgradeInfo.incCost, "마나");
                break;

            case UpgradeInfo.upgradeType.MpRegen: 
                _player.MpRegen += upgradeInfo.incValue;
                SetUI(myUI.level, myUI.value, myUI.cost, upgradeInfo.level, _player.MpRegen, upgradeInfo.incValue, upgradeInfo.incCost, "마나 재생속도");
                break;
        }

        upgradeInfo.level += 1;
    }

    private void SetUI(Text level, Text value, Text cost, int Lv ,float initValue ,float increase, float costInc, string name)
    {
        //UI 세팅
        if (name == "크리티컬 확률")
        {
            level.text = "Lv" + (Lv + 1).ToString() + " " + name;
            value.text = initValue.ToString() + "%" + " -> " + (initValue + increase).ToString() + "%";
            cost.text = Mathf.Round((float.Parse(cost.text) * costInc)).ToString();
            return;
        }

        else if(name == "체력 재생속도" || name =="마나 재생속도")
        {
            level.text = "Lv" + (Lv + 1).ToString() + " " + name;
            value.text = "초당 " + Math.Round(initValue, 1).ToString() + " -> " + Math.Round((initValue + increase), 1).ToString();
            cost.text = Mathf.Round((float.Parse(cost.text) * costInc)).ToString();
            return;
        }

        level.text = "Lv" + (Lv + 1 ).ToString() + " " + name;
        value.text = Math.Round(initValue, 1).ToString() + " -> " + Math.Round(initValue + increase,1).ToString();
        cost.text = Mathf.Round((float.Parse(cost.text) * costInc)).ToString();
    }
}
