using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class ScriptableItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public string itemAbility;
    public string itemCost;
}
