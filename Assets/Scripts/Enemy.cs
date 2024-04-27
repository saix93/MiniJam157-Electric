using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public Enemy_SO Config;

    protected override void Awake()
    {
        base.Awake();

        foreach (Transform child in DiceParent)
        {
            Destroy(child.gameObject);
        }
    }

    protected override void Start()
    {
        MaxBattery = Config.BatteryCapacity;
        currentBattery = Config.InitialBattery;
    }

    public override void StartTurn()
    {
        base.StartTurn();
        
        EndTurn();
    }

    public override void EndTurn()
    {
        // var usedDice = GetDiceFromTransform(DiceParent);
        //
        // var usedAmps = SumDice(usedDice.FindAll(d => d.Type == DieType.Amps));
        // var usedVolts = SumDice(usedDice.FindAll(d => d.Type == DieType.Volts));
        
        gm.EndEnemyTurn();
    }

    protected override void BatteryEmpty()
    {
        gm.EnemyEliminated();
        Debug.LogError($"Enemy done!");
    }
}
