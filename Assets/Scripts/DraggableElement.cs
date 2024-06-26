using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image Image;
    
    public Transform OriginalParent { get; set; }
    public Transform ParentAfterDrag { get; set; }

    private void Awake()
    {
        OriginalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.PlayerCanGrabDice) return;
        
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.PlayerCanGrabDice) return;
        
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.PlayerCanGrabDice) return;
        
        transform.SetParent(ParentAfterDrag);
        Image.raycastTarget = true;
    }

    public void ResetToOriginalParent()
    {
        transform.SetParent(OriginalParent);
    }
}
