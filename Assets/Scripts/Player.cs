using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : Character
{
    public int DiceRerollsPerTurn = 3;
    public int AbilityRerollsPerTurn = 1;
    public Button DiceRerollButton;
    public Button AbilityRerollButton;
    public TextMeshProUGUI DiceRerollButtonLabel;
    public TextMeshProUGUI AbilityRerollButtonLabel;
    public List<Player_SO> Configs;

    private Player_SO currentConfig;
    private int currentDiceRerolls;
    private int currentAbilitiesRerolls;

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
        currentDiceRerolls--;
        UpdateRerollDiceButtonText();
        
        foreach (var die in dice)
        {
            die.ResetToOriginalParent();
            die.Roll();
        }

        if (currentDiceRerolls == 0)
        {
            DiceRerollButton.interactable = false;
        }
    }

    public void RerollAbilities()
    {
        currentAbilitiesRerolls--;
        UpdateRerollAbilitiesButtonText();
        
        foreach (var die in dice)
        {
            die.ResetToOriginalParent();
        }
        
        var abilitiesList = new List<Ability_SO>(abilities)
            .Except(currentAbilities.Where(a => a != null).Select(a => a.Config))
            .Except(abilities.Where(a => abilitiesUsedThisTurn.Contains(a))).ToList();
        SpawnAbilities(abilitiesList, currentAbilities.Count(a => a != null));

        if (currentAbilitiesRerolls == 0)
        {
            AbilityRerollButton.interactable = false;
        }
    }

    private void UpdateRerollDiceButtonText()
    {
        DiceRerollButtonLabel.text = $"Reroll dice ({currentDiceRerolls})";
    }

    private void UpdateRerollAbilitiesButtonText()
    {
        AbilityRerollButtonLabel.text = $"Reroll abilities ({currentAbilitiesRerolls})";
    }

    public override void StartTurn()
    {
        base.StartTurn();

        currentDiceRerolls = DiceRerollsPerTurn;
        currentAbilitiesRerolls = AbilityRerollsPerTurn;
        UpdateRerollDiceButtonText();
        UpdateRerollAbilitiesButtonText();
        DiceRerollButton.interactable = true;
        AbilityRerollButton.interactable = true;
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
