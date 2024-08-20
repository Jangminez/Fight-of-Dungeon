using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSword : MonoBehaviour, IEquipmentable
{
    public void Equipped()
    {
        GameManager.Instance.player.Attack += 30f;
    }
    public void Detachment()
    {
        GameManager.Instance.player.Attack -= 30f;
    }
}
