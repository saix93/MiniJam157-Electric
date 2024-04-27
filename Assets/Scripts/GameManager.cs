using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Header("Config")]
    public MeasurementsConfig Measurements;
    public List<DieTypeColorPair> DiceColors;

    [Header("References")]
    public RectTransform DiceParent;
    public Die DiePrefab;
    public Ability AbilityPrefab;
    public RectTransform AbilitiesParent;
    public Player Player;
    public Enemy Enemy;
    
    [Header("Parameters")]
    public int Turns = 3;

    public float TimeBetweenPhases = 5f;

    private bool inCombat;

    private int MillisecondsBetweenPhases => (int)(TimeBetweenPhases * 1000);
    public GamePhases CurrentPhase { get; set; }
    public GamePhases DebugCurrentPhase;

    private void Update()
    {
        DebugCurrentPhase = CurrentPhase;
    }

    public Color GetDieColor(DieType type)
    {
        return DiceColors.Find(dc => dc.Type == type).Color;
    }

    public void StartCombat()
    {
        inCombat = true;
        Player.StartTurn();
    }

    private void StartEnemyTurn()
    {
        Enemy.StartTurn();
    }

    private async void StartResolutionPhase()
    {
        CurrentPhase = GamePhases.ResolutionPhase;
        
        
        
        if (inCombat)
        {
            await Task.Delay(MillisecondsBetweenPhases);
            Player.StartTurn();
        }
        else
        {
            Debug.LogError($"Se acabo el combate");
        }
    }

    public async void EndPlayerTurn()
    {
        await Task.Delay(MillisecondsBetweenPhases);
        StartEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        StartResolutionPhase();
    }

    public void PlayerEliminated()
    {
        inCombat = false;
        Debug.LogError($"You lose!");
    }

    public void EnemyEliminated()
    {
        inCombat = false;
        Debug.LogError($"You win!");
    }
    
    [Serializable]
    public class DieTypeColorPair
    {
        public DieType Type;
        public Color Color;
    }
}

public enum GamePhases
{
    Waiting,
    DecisionPhase,
    ResolutionPhase,
}

