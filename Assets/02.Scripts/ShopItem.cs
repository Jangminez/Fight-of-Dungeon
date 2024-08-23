using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public ScriptableItem _myItem;
    public Transform _information;
    private Button _myBtn;
    [SerializeField]private Button _buyBtn;


    void Awake()
    {
        _myBtn = GetComponent<Button>();
        _myBtn.onClick.AddListener(ClickItem);
        _buyBtn = _information.GetChild(4).GetComponent<Button>();
    }
    void ClickItem() 
    {
        // 아이템 정보 UI에 표시
        SetUI();

        //_information.GetChild(2).GetComponent<Text>().text = _myItem.itemAbility;

        //_information.GetChild(5).GetComponent<Text>().text = _myItem.requireItem;

        // 기존 리스너 제거 후 새로 할당
        _buyBtn.onClick.RemoveAllListeners();
        _buyBtn.onClick.AddListener(BuyItem);
    }

    void SetUI() 
    {
        _information.gameObject.SetActive(true);
        _information.GetChild(0).GetComponent<Text>().text = _myItem.itemName;
        _information.GetChild(1).GetComponent<Text>().text = _myItem.itemDescription;
        _information.GetChild(3).GetComponent<Text>().text = _myItem.itemCost;

        switch(_myItem.valueType)
        {
            case ScriptableItem.ValueType.Attack:
                _information.GetChild(2).GetComponent<Text>().text = $"공격력 +{_myItem.incValue}";
                break;

            case ScriptableItem.ValueType.AttackSpeed:
                _information.GetChild(2).GetComponent<Text>().text = $"공격속도 +{_myItem.incValue}";
                break;

            case ScriptableItem.ValueType.Critical:
                _information.GetChild(2).GetComponent<Text>().text = $"크리티컬 확률 +{_myItem.incValue}";
                break;

            case ScriptableItem.ValueType.Defense:
                _information.GetChild(2).GetComponent<Text>().text = $"방어력 +{_myItem.incValue}";
                break;

            case ScriptableItem.ValueType.Hp:
                _information.GetChild(2).GetComponent<Text>().text = $"체력 +{_myItem.incValue}";
                break;

            case ScriptableItem.ValueType.HpRegen:
                _information.GetChild(2).GetComponent<Text>().text = $"체력 재생속도 +{_myItem.incValue}";
                break;

            case ScriptableItem.ValueType.Mp:
                _information.GetChild(2).GetComponent<Text>().text = $"마나 +{_myItem.incValue}";
                break;

            case ScriptableItem.ValueType.MpRegen:
                _information.GetChild(2).GetComponent<Text>().text = $"마나 재생속도 +{_myItem.incValue}";
                break;
        }

        if(_myItem.calType == ScriptableItem.CalType.Percentage)
        {
            _information.GetChild(2).GetComponent<Text>().text += "%";
        }
    }
    void BuyItem()
    {
        // 플레이어의 골드가 충분하다면 구매
        if(GameManager.Instance.player.Gold >= Int32.Parse(_myItem.itemCost) && _myItem.needItem.Count == 0)
        {
            Inventory.Instance.AddInventory(_myItem);
        }

        else if(GameManager.Instance.player.Gold >= Int32.Parse(_myItem.itemCost) && _myItem.needItem.Count > 0)
        {
            // 인벤토리에 제작에 필요한 아이템이 있다면 담을 변수 생성
            List<Transform> items;
            items = new List<Transform>();

            // 필요 아이템 반복
            foreach(var item in _myItem.needItem)
            {
                // 해당 아이템이 인벤토리 슬롯에 존재하는지 확인
                foreach(var slot in Inventory.Instance._slots)
                {
                    // 존재한다면 위 변수에 추가 후 탐색 할 아이템 변경
                    if(slot.childCount > 0 && item == slot.GetChild(0).GetComponent<Equipment>()._item){
                        items.Add(slot.GetChild(0)); 
                        break; 
                    }
                }
            }
            // 아이템이 모두 존재한다면 해당 아이템 슬롯에서 제거 & 구매 아이템 추가
            if(items.Count == _myItem.needItem.Count){
                foreach (var item in items){
                    item.GetComponent<Equipment>().UnEquipmentItem();
                }
                Inventory.Instance.AddInventory(_myItem);
            }
        }
    }
}
