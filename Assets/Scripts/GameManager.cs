using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Header("Config")]
    public List<DieTypeColorPair> DiceColors;
    public List<EffectTypeConfig> EffectConfigs;

    [Header("References")]
    public Die DiePrefab;
    public Ability AbilityPrefab;
    public RectTransform AbilitiesParent;
    public TextMeshProUGUI TurnsLabel;
    public Button EndTurnButton;
    public Player Player;
    public Enemy Enemy;
    public GameObject WinBox;
    public GameObject LoseBox;
    
    [Header("Parameters")]
    public int MaxTurns = 3;
    public TimersConfig Timers;

    private int currentTurn;
    private bool playerLost;

    public bool PlayerCanGrabDice { get; private set; }
    private GamePhases currentPhase { get; set; }

    private void Start()
    {
        foreach (Transform child in AbilitiesParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        StartCombat();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
        }
    }

    public Color GetDieColor(DieType type)
    {
        return DiceColors.Find(dc => dc.Type == type).Color;
    }

    public EffectTypeConfig GetEffectConfig(AbilityEffect effect)
    {
        return EffectConfigs.Find(e => e.Type == effect.Type);
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

    private void StartCombat()
    {
        playerLost = false;
        ChangePhase(GamePhases.PlayerPhase, 0f);
    }

    private void ChangePhase(GamePhases newPhase, float time)
    {
        if (playerLost) return;
        
        StopAllCoroutines();
        StartCoroutine(ChangePhaseCR(newPhase, time));
    }

    private IEnumerator ChangePhaseCR(GamePhases newPhase, float time)
    {
        yield return new WaitForSeconds(time);
        
        currentPhase = newPhase;

        switch (currentPhase)
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
        EndTurnButton.interactable = true;
        PlayerCanGrabDice = true;
        
        currentTurn++;

        TurnsLabel.text = $"{currentTurn} / {MaxTurns}";
        
        Player.StartTurn();
    }
    public void EndPlayerTurn()
    {
        EndTurnButton.interactable = false;
        PlayerCanGrabDice = false;
        
        if (currentTurn == MaxTurns)
        {
            TurnsLabel.text = $"Out of time!";
            TurnsLabel.color = Color.red;
            LoseBox.SetActive(true);
            
            ChangePhase(GamePhases.Lose, Timers.Lose);
        }
        else
        {
            ChangePhase(GamePhases.EnemyPhase, Timers.Enemy);
        }
    }

    private void EnemyPhase()
    {
        Enemy.StartTurn();
    }
    public void EndEnemyTurn()
    {
        ChangePhase(GamePhases.PlayerPhase, Timers.Player);
    }

    private void WinPhase()
    {
        SceneManager.LoadScene(1);
    }

    private void LosePhase()
    {
        SceneManager.LoadScene(1);
    }
    #endregion

    public void PlayerEliminated()
    {
        ChangePhase(GamePhases.Lose, Timers.Lose);
        LoseBox.SetActive(true);
        playerLost = true;
    }

    public void EnemyEliminated()
    {
        EndTurnButton.interactable = false;
        PlayerCanGrabDice = false;
        WinBox.SetActive(true);
        ChangePhase(GamePhases.Win, Timers.Win);
    }
    
    [Serializable]
    public class DieTypeColorPair
    {
        public DieType Type;
        public Color Color;
    }

    [Serializable]
    public class EffectTypeConfig
    {
        public AbilityEffectType Type;
        public Sprite Sprite;
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

