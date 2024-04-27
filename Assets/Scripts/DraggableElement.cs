using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image Image;
    
    public Transform ParentAfterDrag { get; set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentAfterDrag);
        Image.raycastTarget = true;
    }
}
