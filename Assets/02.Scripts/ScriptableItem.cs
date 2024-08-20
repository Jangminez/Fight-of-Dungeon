using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class ScriptableItem : ScriptableObject
{
    public GameObject item;
    public string itemName;
    public string itemDescription;
    public string itemAbility;
    public string itemCost;
    public string requireItem;
}
