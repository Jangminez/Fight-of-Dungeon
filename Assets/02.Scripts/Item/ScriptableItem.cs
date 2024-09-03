using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class ScriptableItem : ScriptableObject
{
    public GameObject item;
    public string itemName;
    public enum ValueType {Attack, AttackSpeed, Critical, Defense, Hp, HpRegen, Mp, MpRegen}
    public enum CalType {Plus, Percentage}
    public List<Stat> statsList = new List<Stat>();
    public string itemDescription;
    public string itemCost;
    public List<ScriptableItem> needItem;

    [NonSerialized]
    public Dictionary<Tuple<ValueType, CalType>, float> stats;

    [Serializable]
    public class Stat
    {
        public ValueType type;
        public CalType caltype;
        public float value;
    }

    void OnEnable()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        stats = new Dictionary<Tuple<ValueType, CalType>, float>();
        foreach(Stat stat in statsList)
        {
            var key = new Tuple<ValueType, CalType>(stat.type, stat.caltype);
            if(!stats.ContainsKey(key))
            {
                stats.Add(key, stat.value);
            }
        }
    }

}

