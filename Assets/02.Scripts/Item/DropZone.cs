using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public enum ZoneType {Drop, Sell};
    public ZoneType _type;

    public void OnDrop(PointerEventData eventData)
    {
        Equipment dragItem = eventData.pointerDrag.GetComponent<Equipment>();

        if(dragItem != null)
        {
            switch(_type)
            {
                case ZoneType.Drop:
                    dragItem.UnEquipmentItem();
                    Debug.Log("아이템 드랍");
                    break;

                case ZoneType.Sell:
                    dragItem.SellItem();
                    Debug.Log("아이템 판매");
                    break;
            }
        }
    }
}
