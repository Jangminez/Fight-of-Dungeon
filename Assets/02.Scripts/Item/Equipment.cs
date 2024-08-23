using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public ScriptableItem _item;
    private Button _myBtn;
    void Awake()
    {
        _myBtn = this.transform.parent.GetComponent<Button>();
        _myBtn.onClick.AddListener(ClickButton);
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

    protected void ClickButton(){

    }
}
