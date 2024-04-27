using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DieSlot : DragDropTargetElement
{
    public Image Image;

    public override void OnDrop(PointerEventData eventData)
    {
        // TODO: Comprobar que se cumplen los requerimientos
        
        base.OnDrop(eventData);
    }
}
