using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public ScriptableItem _item;
    [HideInInspector] public Button _slotBtn;
    [SerializeField] private Button _sellBtn;
    public Button SellBtn => _sellBtn;
    void Awake()
    {
        _slotBtn = this.transform.parent.GetComponent<Button>();
        _sellBtn = this.transform.GetChild(0).GetComponent<Button>();

        if(_slotBtn != null && _sellBtn != null){
            _slotBtn.onClick.AddListener(ClickSlot);
            _sellBtn.onClick.AddListener(SellItem);
        }
    }
    void FixedUpdate()
    {
        //다른 버튼 클릭 시 판매 버튼 비활성화
        GameObject selectOb = EventSystem.current.currentSelectedGameObject;

        if(_sellBtn.gameObject.activeSelf && selectOb != _slotBtn.gameObject && selectOb != _sellBtn.gameObject ){
            _sellBtn.gameObject.SetActive(false);
       }
    }

    // 장비 장착 시
    public void EquipmentItem()
    {
        if(_item.calType == ScriptableItem.CalType.Plus){
            switch(_item.valueType)
            {
                case ScriptableItem.ValueType.Attack:
                    GameManager.Instance.player.Attack += _item.incValue;
                break;

                case ScriptableItem.ValueType.AttackSpeed:
                    GameManager.Instance.player.AttackSpeed += _item.incValue;
                break;

                case ScriptableItem.ValueType.Critical:
                    GameManager.Instance.player.Critical += _item.incValue;
                break;

                case ScriptableItem.ValueType.Defense:
                    GameManager.Instance.player.Defense += _item.incValue;
                break;

                case ScriptableItem.ValueType.Hp:
                    GameManager.Instance.player.MaxHp += _item.incValue;
                break;

                case ScriptableItem.ValueType.HpRegen:
                    GameManager.Instance.player.HpRegen += _item.incValue;
                break;

                case ScriptableItem.ValueType.Mp:
                    GameManager.Instance.player.MaxMp += _item.incValue;
                break;

                case ScriptableItem.ValueType.MpRegen:
                    GameManager.Instance.player.MpRegen += _item.incValue;
                break;
            }
        }

        else if(_item.calType == ScriptableItem.CalType.Percentage){
            switch(_item.valueType)
            {
                case ScriptableItem.ValueType.Attack:
                    GameManager.Instance.player.AttackBonus += _item.incValue;
                break;

                case ScriptableItem.ValueType.AttackSpeed:
                    GameManager.Instance.player.AsBonus += _item.incValue;
                break;

                case ScriptableItem.ValueType.Critical:
                    Debug.Log("Wrong Setting!!");
                break;

                case ScriptableItem.ValueType.Defense:
                    GameManager.Instance.player.DefenseBonus += _item.incValue;
                break;

                case ScriptableItem.ValueType.Hp:
                    GameManager.Instance.player.HpBonus += _item.incValue;
                break;

                case ScriptableItem.ValueType.HpRegen:
                    GameManager.Instance.player.HpRegenBonus += _item.incValue;
                break;

                case ScriptableItem.ValueType.Mp:
                    GameManager.Instance.player.MpBonus += _item.incValue;
                 break;

                case ScriptableItem.ValueType.MpRegen:
                    GameManager.Instance.player.MpRegenBonus += _item.incValue;
                break;
            }
        }
    }

    // 장비 해제 시
    public void UnEquipmentItem()
    {
        if(_item.calType == ScriptableItem.CalType.Plus){
            switch(_item.valueType)
            {
                case ScriptableItem.ValueType.Attack:
                    GameManager.Instance.player.Attack -= _item.incValue;
                break;

                case ScriptableItem.ValueType.AttackSpeed:
                    GameManager.Instance.player.AttackSpeed -= _item.incValue;
                break;

                case ScriptableItem.ValueType.Critical:
                    GameManager.Instance.player.Critical -= _item.incValue;
                break;

                case ScriptableItem.ValueType.Defense:
                    GameManager.Instance.player.Defense -= _item.incValue;
                break;

                case ScriptableItem.ValueType.Hp:
                    GameManager.Instance.player.MaxHp -= _item.incValue;
                break;

                case ScriptableItem.ValueType.HpRegen:
                    GameManager.Instance.player.HpRegen -= _item.incValue;
                break;

                case ScriptableItem.ValueType.Mp:
                    GameManager.Instance.player.MaxMp -= _item.incValue;
                break;

                case ScriptableItem.ValueType.MpRegen:
                    GameManager.Instance.player.MpRegen -= _item.incValue;
                break;
            }
        }

        else if(_item.calType == ScriptableItem.CalType.Percentage){
            switch(_item.valueType)
            {
                case ScriptableItem.ValueType.Attack:
                    GameManager.Instance.player.AttackBonus -= _item.incValue;
                break;

                case ScriptableItem.ValueType.AttackSpeed:
                    GameManager.Instance.player.AsBonus -= _item.incValue;
                break;

                case ScriptableItem.ValueType.Critical:
                    Debug.Log("Wrong Setting!!");
                break;

                case ScriptableItem.ValueType.Defense:
                    GameManager.Instance.player.DefenseBonus -= _item.incValue;
                break;

                case ScriptableItem.ValueType.Hp:
                    GameManager.Instance.player.HpBonus -= _item.incValue;
                break;

                case ScriptableItem.ValueType.HpRegen:
                    GameManager.Instance.player.HpRegenBonus -= _item.incValue;
                break;

                case ScriptableItem.ValueType.Mp:
                    GameManager.Instance.player.MpBonus -= _item.incValue;
                    
                 break;

                case ScriptableItem.ValueType.MpRegen:
                    GameManager.Instance.player.MpRegenBonus -= _item.incValue;
                break;
            }
        }

        Destroy(this.gameObject);
    }

    // 슬롯 버튼 클릭시
    public void ClickSlot(){
        _sellBtn.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_sellBtn.gameObject);
    }

    void SellItem() {
        UnEquipmentItem();
        // 구매 가격의 80% 반환
        GameManager.Instance.player.Gold += Mathf.RoundToInt(Int32.Parse(_item.itemCost) * 0.8f);
    }
}