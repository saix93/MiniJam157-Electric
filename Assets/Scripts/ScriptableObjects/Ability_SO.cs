using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability_", menuName = "--- Electric/Ability", order = 102)]
public class Ability_SO : ScriptableObject
{
    public string Name;
    public List<DieRequeriment> Dice;
    public List<AbilityEffect> Effects;
}

[Serializable]
public struct DieRequeriment
{
    public DieRequerimentType RequerimentType;
    public DieType DieType;
    public int Value;
}

public enum DieRequerimentType
{
    Only,
    Not,
    BelowAnd,
    OverAnd,
    Odd,
    Even,
    Any
}

[Serializable]
public struct AbilityEffect
{
    public AbilityEffectType Type;
    public int Value;
}

public enum AbilityEffectType
{
    TransferEnergy,
    GenerateEnergy,
}
