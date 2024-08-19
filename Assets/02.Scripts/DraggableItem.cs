using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image _img;
    [HideInInspector] public Transform _preParent;
    public void OnBeginDrag(PointerEventData eventData)
    {
        _img = GetComponent<Image>();
        _preParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _img.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_preParent);
        _img.raycastTarget = true;
    }
}
