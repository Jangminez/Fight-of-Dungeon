using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class ScriptableItem : ScriptableObject
{
    public GameObject item;
    public string itemName;
    public enum ValueType {Attack, AttackSpeed, Critical, Defense, Hp, HpRegen, Mp, MpRegen}
    public enum CalType {Plus, Percentage}
    public ValueType valueType;
    public CalType calType;
    public float incValue;
    public string itemDescription;
    public string itemCost;
    public List<ScriptableItem> needItem;
}

