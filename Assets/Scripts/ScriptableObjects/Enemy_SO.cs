using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_", menuName = "--- Electric/Enemy", order = 100)]
public class Enemy_SO : ScriptableObject
{
    public string Name;
    public List<DieType> DiceConfig;
    public List<Ability_SO> Abilities;
    public int BatteryCapacity = 20;
    public int InitialBattery = 5;
    public int MaxTurns = 3;
    public Sprite Sprite;
}
