using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public ScriptableItem _myItem;
    public Transform Information;
    private Button _myBtn;
    [SerializeField]private Button _buyBtn;


    void Awake()
    {
        _myBtn = GetComponent<Button>();
        _myBtn.onClick.AddListener(ClickItem);
        _buyBtn = Information.GetChild(4).GetComponent<Button>();
    }

    void ClickItem() 
    {
        // 아이템 정보 UI에 표시
        Information.gameObject.SetActive(true);
        Information.GetChild(0).GetComponent<Text>().text = _myItem.itemName;
        Information.GetChild(1).GetComponent<Text>().text = _myItem.itemDescription;
        Information.GetChild(2).GetComponent<Text>().text = _myItem.itemAbility;
        Information.GetChild(3).GetComponent<Text>().text = _myItem.itemCost;
        Information.GetChild(5).GetComponent<Text>().text = _myItem.requireItem;

        // 기존 리스너 제거 후 새로 할당
        _buyBtn.onClick.RemoveAllListeners();
        _buyBtn.onClick.AddListener(BuyItem);
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
                    Destroy(item.gameObject);
                }
                Inventory.Instance.AddInventory(_myItem);
            }
        }
    }
}
