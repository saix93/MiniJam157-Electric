using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_", menuName = "--- Electric/Enemy", order = 100)]
public class Enemy_SO : ScriptableObject
{
    public DiceConfig_SO DiceConfig;
    public int BatteryCapacity = 20;
    public int InitialBattery = 5;
}
