using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Config")]
    public MeasurementsConfig Measurements;
    public DiceConfig_SO DiceConfig;

    // [Header("3D References")]
    // public Transform AmpsDie3DPosition;
    // public Transform VoltsDie3DPosition;
    // public Die3D AmpsDie3D;
    // public Die3D VoltsDie3D;

    [Header("2D References")]
    public RectTransform Dice2DParent;
    public Die2D Die2DPrefab;
    
    [Header("Parameters")]
    public float HoursPerTurn = 1f;
    public int Turns = 3;
    
    [Header("Debug")]
    public float PowerFactor = 1f;
    
    public float Watts;
    public float MAh;

    private List<Die2D> dice;
    
    public float CurrentVolts { get; private set; }
    public float CurrentAmps { get; private set; }
    public float PredictedVolts { get; private set; }
    public float PredictedAmps { get; private set; }

    private void Start()
    {
        dice = new List<Die2D>();
        
        foreach (var die in DiceConfig.Dice)
        {
            var die2D = Instantiate(Die2DPrefab, Dice2DParent);
            die2D.LoadConfig(die);
            die2D.Clear();
            
            dice.Add(die2D);
        }
    }

    private void Update()
    {
        PredictedVolts = CurrentVolts;
        PredictedAmps = CurrentAmps;
        
        foreach (var die in dice)
        {
            if (die.CurrentSide is null) continue;

            foreach (var effect in die.CurrentSide.Effects)
            {
                switch (effect.Type)
                {
                    case DieSideEffectType.None:
                        break;
                    case DieSideEffectType.Volts:
                        PredictedVolts += effect.Value;
                        break;
                    case DieSideEffectType.Amps:
                        PredictedAmps += effect.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        Watts = CalcWatts(PredictedAmps, PredictedVolts, PowerFactor);
        MAh = CalcMilliampsPerHour(Watts * HoursPerTurn, PredictedVolts);

        // if (!Use3DDice)
        // {
        //     AmpsDie3D.gameObject.SetActive(false);
        //     VoltsDie3D.gameObject.SetActive(false);
        // }
    }

    public void RollDice()
    {
        // if (Use3DDice)
        // {
        //     AmpsDie3D.ResetRigidbody();
        //     VoltsDie3D.ResetRigidbody();
        //
        //     var ampsT = AmpsDie3DPosition.transform;
        //     var voltsT = VoltsDie3DPosition.transform;
        //
        //     AmpsDie3D.transform.position = ampsT.position;
        //     AmpsDie3D.transform.rotation = ampsT.rotation;
        //     VoltsDie3D.transform.position = voltsT.position;
        //     VoltsDie3D.transform.rotation = voltsT.rotation;
        //
        //     AmpsDie3D.AddForces();
        //     VoltsDie3D.AddForces();
        // }

        foreach (var die in dice)
        {
            die.gameObject.SetActive(true);
            if (die.IsBlocked) continue;
            
            die.Roll();
        }
    }

    public void LockDice()
    {
        foreach (var die in dice)
        {
            foreach (var effect in die.CurrentSide.Effects)
            {
                switch (effect.Type)
                {
                    case DieSideEffectType.None:
                        break;
                    case DieSideEffectType.Volts:
                        CurrentVolts += effect.Value;
                        break;
                    case DieSideEffectType.Amps:
                        CurrentAmps += effect.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            die.Clear();
        }
    }

    public void EndTurn()
    {
        ProcessLastTurn();
        
        CurrentVolts = 0f;
        CurrentAmps = 0f;
        PredictedVolts = 0f;
        PredictedAmps = 0f;

        foreach (var die in dice)
        {
            die.Clear();
        }
    }

    private void ProcessLastTurn()
    {
        // TODO
    }

    private float CalcWatts(float amps, float volts, float powerFactor)
    {
        return amps * volts * powerFactor;
    }

    private float CalcMilliampsPerHour(float wattsPerHour, float volts)
    {
        return wattsPerHour * 1000f / volts;
    }
}
