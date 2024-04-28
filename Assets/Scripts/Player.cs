using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public void RerollDice()
    {
        foreach (var die in dice)
        {
            die.ResetToOriginalParent();
            die.Roll();
        }
    }

    public override void EndTurn()
    {
        Debug.Log($"----- End turn...");
        gm.EndPlayerTurn();
    }

    protected override void BatteryFull()
    {
        
    }

    protected override void BatteryEmpty()
    {
        gm.PlayerEliminated();
        Debug.LogError($"Lose game!");
    }
}
