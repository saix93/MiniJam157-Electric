using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceConfig_", menuName = "--- Electric/DiceConfig", order = 101)]
public class DiceConfig_SO : ScriptableObject
{
    public List<DieType> Dice;
}
