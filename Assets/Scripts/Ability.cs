using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public GridLayoutGroup DiceBox;
    public DieSlot DieSlotPrefab;
    public RectTransform Description;
    public TextMeshProUGUI AbilityEffectPrefab;
    
    private Ability_SO config;
    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void Init(Ability_SO newConfig)
    {
        config = newConfig;

        foreach (Transform child in DiceBox.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in Description.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var die in config.Dice)
        {
            var dieSlot = Instantiate(DieSlotPrefab, DiceBox.transform);
            dieSlot.Init(die);
        }

        foreach (var effect in config.Effects)
        {
            var effectLabel = Instantiate(AbilityEffectPrefab, Description);
            effectLabel.text = $"{effect.Value} - {effect.Type}";
        }
    }

    public void UseAbility()
    {
        
    }
}
