using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HideUI : MonoBehaviour, IPointerClickHandler
{
    public Transform _information;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        _information.gameObject.SetActive(false);
    }


}