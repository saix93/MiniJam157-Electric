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
    public TimersConfig Timers;

    private bool inCombat;

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
    
    public List<T> GetShuffledList<T>(List<T> list)
    {
        var shuffledList = new List<T>(list);
        
        for (var i = 0; i < shuffledList.Count; i++)
        {
            var randomIndex = Random.Range(i, shuffledList.Count);
            T temp = shuffledList[i];
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }
        
        return shuffledList;
    }

    public void StartCombat()
    {
        inCombat = true;
        
        StartCoroutine(ChangePhase(GamePhases.PlayerPhase, Timers.Player));
    }

    private IEnumerator ChangePhase(GamePhases newPhase, float time)
    {
        CurrentPhase = newPhase;

        yield return new WaitForSeconds(time);

        switch (CurrentPhase)
        {
            case GamePhases.Waiting:
                break;
            case GamePhases.PlayerPhase:
                PlayerPhase();
                break;
            case GamePhases.EnemyPhase:
                EnemyPhase();
                break;
            case GamePhases.Win:
                WinPhase();
                break;
            case GamePhases.Lose:
                LosePhase();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region Phases
    private void PlayerPhase()
    {
        Player.StartTurn();
    }
    public void EndPlayerTurn()
    {
        StartCoroutine(ChangePhase(GamePhases.EnemyPhase, Timers.Enemy));
    }

    private void EnemyPhase()
    {
        Enemy.StartTurn();
    }
    public void EndEnemyTurn()
    {
        StartCoroutine(ChangePhase(GamePhases.PlayerPhase, Timers.Player));
    }

    private void WinPhase()
    {
        Debug.LogError($"You win!");
    }

    private void LosePhase()
    {
        Debug.LogError($"You lose!");
    }
    #endregion


    public void PlayerEliminated()
    {
        inCombat = false;
        StartCoroutine(ChangePhase(GamePhases.Lose, Timers.Lose));
    }

    public void EnemyEliminated()
    {
        inCombat = false;
        StartCoroutine(ChangePhase(GamePhases.Win, Timers.Win));
    }
    
    [Serializable]
    public class DieTypeColorPair
    {
        public DieType Type;
        public Color Color;
    }

    [Serializable]
    public class TimersConfig
    {
        public float Player = 2f;
        public float Enemy = 3f;
        public float Win = 5f;
        public float Lose = 5;
    }
}

public enum GamePhases
{
    Waiting,
    PlayerPhase,
    EnemyPhase,
    Win,
    Lose
}

