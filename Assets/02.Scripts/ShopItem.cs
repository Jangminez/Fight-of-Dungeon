using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IDeselectHandler
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
        _buyBtn.onClick.AddListener(BuyItem);
    }

    void ClickItem() 
    {
        // 아이템 정보 UI에 표시
        Information.gameObject.SetActive(true);
        Information.GetChild(0).GetComponent<Text>().text = _myItem.itemName;
        Information.GetChild(1).GetComponent<Text>().text = _myItem.itemDescription;
        Information.GetChild(2).GetComponent<Text>().text = _myItem.itemAbility;
        Information.GetChild(3).GetComponent<Text>().text = _myItem.itemCost;
    }

    void BuyItem()
    {
        // 플레이어의 골드가 충분하다면 구매
        if(GameManager.Instance.player.Gold >= Int32.Parse(_myItem.itemCost))
        {
            Inventory.Instance.AddInventory(_myItem);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //Invoke("OffInformation", 0.1f);
    }

    void OffInformation(){
        Information.gameObject.SetActive(false);
    }
}
