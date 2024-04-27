using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_", menuName = "--- Electric/Enemy", order = 100)]
public class Enemy_SO : ScriptableObject
{
    public int BatteryCapacity = 20;
    public int InitialBattery = 5;
}
