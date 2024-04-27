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
            die.Roll();
            var rollResult = die.CurrentValue;
            Debug.Log("Die rolled: " + rollResult);
            // Perform any actions based on the roll result here
        }
    }

    public override void EndTurn()
    {
        // var attackDice = GetDiceFromTransform(AttackContainer);
        // var defenseDice = GetDiceFromTransform(DefenseContainer);
        //
        // var attackAmps = SumDice(attackDice.FindAll(d => d.Type == DieType.Amps));
        // var attackVolts = SumDice(attackDice.FindAll(d => d.Type == DieType.Volts));
        //
        // var defenseAmps = SumDice(defenseDice.FindAll(d => d.Type == DieType.Amps));
        // var defenseVolts = SumDice(defenseDice.FindAll(d => d.Type == DieType.Volts));

        Debug.Log($"----- End turn...");
        gm.EndPlayerTurn();
    }

    protected override void BatteryEmpty()
    {
        gm.PlayerEliminated();
        Debug.LogError($"Lose game!");
    }
}
