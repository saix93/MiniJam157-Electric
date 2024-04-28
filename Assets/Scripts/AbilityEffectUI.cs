using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityEffectUI : MonoBehaviour
{
    public TextMeshProUGUI Label;
    public Image Icon;

    private GameManager.EffectTypeConfig config;

    public void Init(GameManager.EffectTypeConfig newConfig, AbilityEffect effect)
    {
        config = newConfig;

        var description = "";

        switch (effect.Type)
        {
            case AbilityEffectType.TransferEnergy:
                description += $"Transfers ";
                break;
            case AbilityEffectType.GenerateEnergy:
                description += $"Generates ";
                break;
            case AbilityEffectType.WasteEnergy:
                description += $"Destroys ";
                break;
            case AbilityEffectType.AddResistance:
                description += $"Adds ";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        description += $"{effect.Value}";
        Label.text = description;
        Label.color = config.Color;
        
        Icon.sprite = config.Sprite;
        Icon.color = config.Color;
    }
}
