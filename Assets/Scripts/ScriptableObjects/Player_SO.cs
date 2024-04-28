using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_", menuName = "--- Electric/Player", order = 101)]
public class Player_SO : ScriptableObject
{
    public string Name;
    public List<DieType> DiceConfig;
    public List<Ability_SO> Abilities;
    public int BatteryCapacity = 10;
    public int InitialBattery = 10;
    public Sprite Sprite;
}
