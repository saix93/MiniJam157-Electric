using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropTargetElement : MonoBehaviour, IDropHandler
{
    //public int MaxElements = 2;

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        //if (transform.childCount == MaxElements) return;
        
        var go = eventData.pointerDrag;
        var dragElement = go.GetComponent<DraggableElement>();
        dragElement.ParentAfterDrag = transform;
    }
}
