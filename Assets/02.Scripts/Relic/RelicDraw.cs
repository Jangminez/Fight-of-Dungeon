using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicDraw : MonoBehaviour
{
    private List<ScriptableRelic> relics;
    private int randomInt;

    void Awake()
    {
        Resources.LoadAll<ScriptableRelic>("Relics");
    }

    private int DrawRelic()
    {
        return Random.Range(0, relics.Count);
    }
}
