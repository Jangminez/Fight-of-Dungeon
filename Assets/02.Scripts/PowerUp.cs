using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    private Button myBtn;

    void Awake()
    {
        myBtn = GetComponent<Button>();

        myBtn.onClick.AddListener(GameManager.Instance.GetPowerUp);
    }
}
