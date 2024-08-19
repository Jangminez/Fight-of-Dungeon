using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSword : MonoBehaviour, IEquipmentable
{
    public void Equipped()
    {
        GameManager.Instance.player.Attack += 10f;
    }
    public void Detachment()
    {
        GameManager.Instance.player.Attack -= 10f;
    }

}
