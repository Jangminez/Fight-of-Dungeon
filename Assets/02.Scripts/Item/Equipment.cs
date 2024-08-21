using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public enum ValueType {Attack, AttackSpeed, Critical, Defense, Hp, HpRegen, Mp, MpRegen}
    public enum CalType {Plus, Percentage}
    public ValueType _valueType;
    public CalType _calType;
    public float _value;


// 장비 장착 시
    public void EquipmentItem()
    {
        if(_calType == CalType.Plus){
            switch(_valueType)
            {
                case ValueType.Attack:
                    GameManager.Instance.player.Attack += _value;
                break;

                case ValueType.AttackSpeed:
                    GameManager.Instance.player.AttackSpeed += _value;
                break;

                case ValueType.Critical:
                    GameManager.Instance.player.Critical += _value;
                break;

                case ValueType.Defense:
                    GameManager.Instance.player.Defense += _value;
                break;

                case ValueType.Hp:
                    GameManager.Instance.player.MaxHp += _value;
                break;

                case ValueType.HpRegen:
                    GameManager.Instance.player.HpRegen += _value;
                break;

                case ValueType.Mp:
                    GameManager.Instance.player.MaxMp += _value;
                break;

                case ValueType.MpRegen:
                    GameManager.Instance.player.MpRegen += _value;
                break;
            }
        }

        else if(_calType == CalType.Percentage){
            switch(_valueType)
            {
                case ValueType.Attack:
                    GameManager.Instance.player.AttackBonus += _value;
                break;

                case ValueType.AttackSpeed:
                    GameManager.Instance.player.AsBonus += _value;
                break;

                case ValueType.Critical:
                    Debug.Log("Wrong Setting!!");
                break;

                case ValueType.Defense:
                    GameManager.Instance.player.DefenseBonus += _value;
                break;

                case ValueType.Hp:
                    GameManager.Instance.player.HpBonus += _value;
                break;

                case ValueType.HpRegen:
                    GameManager.Instance.player.HpRegenBonus += _value;
                break;

                case ValueType.Mp:
                    GameManager.Instance.player.MpBonus += _value;
                 break;

                case ValueType.MpRegen:
                    GameManager.Instance.player.MpRegenBonus += _value;
                break;
            }
        }
    }

// 장비 해제 시
    public void UnEquipmentItem() 
    {
        if(_calType == CalType.Plus){
            switch(_valueType)
            {
                case ValueType.Attack:
                    GameManager.Instance.player.Attack -= _value;
                break;

                case ValueType.AttackSpeed:
                    GameManager.Instance.player.AttackSpeed -= _value;
                break;

                case ValueType.Critical:
                    GameManager.Instance.player.Critical -= _value;
                break;

                case ValueType.Defense:
                    GameManager.Instance.player.Defense -= _value;
                break;

                case ValueType.Hp:
                    GameManager.Instance.player.MaxHp -= _value;
                break;

                case ValueType.HpRegen:
                    GameManager.Instance.player.HpRegen -= _value;
                break;

                case ValueType.Mp:
                    GameManager.Instance.player.MaxMp -= _value;
                break;

                case ValueType.MpRegen:
                    GameManager.Instance.player.MpRegen -= _value;
                break;
            }
        }

        else if(_calType == CalType.Percentage){
            switch(_valueType)
            {
                case ValueType.Attack:
                    GameManager.Instance.player.AttackBonus -= _value;
                break;

                case ValueType.AttackSpeed:
                    GameManager.Instance.player.AsBonus -= _value;
                break;

                case ValueType.Critical:
                    Debug.Log("Wrong Setting!!");
                break;

                case ValueType.Defense:
                    GameManager.Instance.player.DefenseBonus -= _value;
                break;

                case ValueType.Hp:
                    GameManager.Instance.player.HpBonus -= _value;
                break;

                case ValueType.HpRegen:
                    GameManager.Instance.player.HpRegenBonus -= _value;
                break;

                case ValueType.Mp:
                    GameManager.Instance.player.MpBonus -= _value;
                    
                 break;

                case ValueType.MpRegen:
                    GameManager.Instance.player.MpRegenBonus -= _value;
                break;
            }
        }
    }
}
