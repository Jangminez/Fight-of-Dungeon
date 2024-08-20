using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendSword : MonoBehaviour, IEquipmentable
{
    public void Equipped()
    {
        GameManager.Instance.player.Attack += 50f;
    }
    public void Detachment()
    {
        GameManager.Instance.player.Attack -= 50f;
    }

}
