using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DieConfig_", menuName = "--- Electric/DieConfig", order = 101)]
public class DieConfig_SO : ScriptableObject
{
    public List<DieSide> Sides;
}

[Serializable]
public class DieSide
{
    public List<DieSideEffect> Effects;
}

[Serializable]
public class DieSideEffect
{
    public DieSideEffectType Type;
    public float Value;
}

public enum DieSideEffectType
{
    None,
    Volts,
    Amps,
}
