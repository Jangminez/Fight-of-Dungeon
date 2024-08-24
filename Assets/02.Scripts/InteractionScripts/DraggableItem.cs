using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image _img;
    [HideInInspector] public Transform _preParent;
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작될 때
        _img = GetComponent<Image>();
        _preParent = transform.parent;
        transform.SetParent(transform.root);
        // 드래그하는 아이템에 UI 최상단에 보이기 위함
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
        SetButton();

        _img.raycastTarget = true;
    }

    void SetButton() {
        Equipment equipment = GetComponent<Equipment>();

        equipment._slotBtn.onClick.RemoveAllListeners();
        equipment._slotBtn = _preParent.GetComponent<Button>();
        equipment._slotBtn.onClick.AddListener(GetComponent<Equipment>().ClickSlot);
    }
}
