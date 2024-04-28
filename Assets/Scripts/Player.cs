using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : Character
{
    public int RerollsPerTurn = 3;
    public Button RerollButton;
    public TextMeshProUGUI RerollButtonLabel;
    public List<Player_SO> Configs;

    private Player_SO currentConfig;
    private int currentRerolls;

    protected override void Start()
    {
        currentConfig = Configs[Random.Range(0, Configs.Count - 1)];
        
        maxBattery = currentConfig.BatteryCapacity;
        currentBattery = currentConfig.InitialBattery;
        dicePool = currentConfig.DiceConfig;
        abilities = new List<Ability_SO>(currentConfig.Abilities);
        CharacterImage.sprite = currentConfig.Sprite;

        NameLabel.text = currentConfig.Name;
        
        base.Start();
    }

    public void RerollDice()
    {
        currentRerolls--;
        UpdateRerollButtonText();
        
        foreach (var die in dice)
        {
            die.ResetToOriginalParent();
            die.Roll();
        }

        if (currentRerolls == 0)
        {
            RerollButton.interactable = false;
        }
    }

    private void UpdateRerollButtonText()
    {
        RerollButtonLabel.text = $"Reroll ({currentRerolls})";
    }

    public override void StartTurn()
    {
        base.StartTurn();

        currentRerolls = RerollsPerTurn;
        UpdateRerollButtonText();
        RerollButton.interactable = true;
    }

    public override void EndTurn()
    {
        foreach (var die in dice)
        {
            die.ResetToOriginalParent();
        }
        
        gm.EndPlayerTurn();
    }

    protected override void BatteryFull()
    {
        
    }

    protected override void BatteryEmpty()
    {
        gm.PlayerEliminated();
    }
}
