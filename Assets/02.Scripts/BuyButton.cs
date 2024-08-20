using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour, IDeselectHandler
{
    private Transform _information;
    private Button _btn;

    void Awake()
    {
        _btn = GetComponent<Button>();
        _information = this.transform.parent;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        _information.gameObject.SetActive(false);
    }
}
