using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DieSlot : DragDropTargetElement
{
    public TextMeshProUGUI Label;
    public Image Image;
    public Transform DieContainer;

    private DieRequeriment config;
    private Die currentDie => GetComponentInChildren<Die>();

    protected override void Awake()
    {
        // base.Awake(); // Evitar que se destruyan los hijos

        targetTransform = DieContainer;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;
        var die = go.GetComponent<Die>();

        var requerimentsPassed = CheckRequirements(die);
        if (!requerimentsPassed) return;

        if (currentDie is not null) currentDie.ResetToOriginalParent();

        base.OnDrop(eventData);
    }

    public void Init(DieRequeriment newConfig)
    {
        config = newConfig;
        
        Image.color = GameManager.Instance.GetDieColor(config.DieType);

        switch (config.RequerimentType)
        {
            case DieRequerimentType.Only:
                Label.text = $"{config.Value}";
                break;
            case DieRequerimentType.Not:
                Label.text = $"!{config.Value}";
                break;
            case DieRequerimentType.BelowAnd:
                Label.text = $"{config.Value}-";
                break;
            case DieRequerimentType.OverAnd:
                Label.text = $"{config.Value}+";
                break;
            case DieRequerimentType.Odd:
                Label.text = $"Odd";
                break;
            case DieRequerimentType.Even:
                Label.text = $"Even";
                break;
            case DieRequerimentType.Any:
                Label.text = $"+";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool CheckRequirements(Die die)
    {
        if (die.Type != config.DieType) return false;

        switch (config.RequerimentType)
        {
            case DieRequerimentType.Only:
                if (die.CurrentValue != config.Value) return false;
                break;
            case DieRequerimentType.Not:
                if (die.CurrentValue == config.Value) return false;
                break;
            case DieRequerimentType.BelowAnd:
                if (die.CurrentValue > config.Value) return false;
                break;
            case DieRequerimentType.OverAnd:
                if (die.CurrentValue < config.Value) return false;
                break;
            case DieRequerimentType.Odd:
                if (die.CurrentValue % 2 == 0) return false;
                break;
            case DieRequerimentType.Even:
                if (die.CurrentValue % 2 != 0) return false;
                break;
            case DieRequerimentType.Any:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return true;
    }
}
