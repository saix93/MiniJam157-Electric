using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class Character : MonoBehaviour
{
    public Image BatteryImage;
    public TextMeshProUGUI BatteryText;
    public RectTransform DiceParent;
    public int MaxBattery = 100;
    public DiceConfig_SO DiceConfig;
    public List<Ability_SO> Abilities;

    [Header("Parameters")]
    public int MaxAbilities = 4;

    private List<DieType> dicePool;

    protected List<Die> dice;
    protected List<Ability> currentAbilities;
    protected GameManager gm;
    
    private float batteryPercentage => (float)currentBattery / (float)MaxBattery;
    protected int currentBattery { get; set; }

    protected virtual void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        dicePool = new List<DieType>(DiceConfig.Dice);
        dice = new List<Die>();
        currentAbilities = new List<Ability>();
    }

    protected virtual void Start()
    {
        currentBattery = MaxBattery;
    }

    private void Update()
    {
        BatteryImage.fillAmount = batteryPercentage;
        BatteryText.text = $"{currentBattery}/{MaxBattery}";
    }
    
    public virtual void StartTurn()
    {
        SpawnAbilities();
        SpawnDice();
    }

    private void SpawnAbilities()
    {
        foreach (Transform child in gm.AbilitiesParent.transform)
        {
            Destroy(child.gameObject);
        }
        currentAbilities.Clear();
        
        var randomAbilities = GetRandomElements(Abilities, MaxAbilities);
        
        foreach (var ability in randomAbilities)
        {
            var newAbility = Instantiate(gm.AbilityPrefab, gm.AbilitiesParent);
            newAbility.Init(ability);
            currentAbilities.Add(newAbility);
        }
    }
    
    private List<T> GetRandomElements<T>(List<T> list, int n)
    {
        var result = new List<T>();
        var indicesPicked = new List<int>();
        
        while (result.Count < n && result.Count < list.Count)
        {
            var randomIndex = Random.Range(0, list.Count);
            if (!indicesPicked.Contains(randomIndex))
            {
                indicesPicked.Add(randomIndex);
                result.Add(list[randomIndex]);
            }
        }
        
        return result;
    }

    private void SpawnDice()
    {
        DestroyDice();
        dice.Clear();

        foreach (var die in dicePool)
        {
            var newDie = Instantiate(gm.DiePrefab, DiceParent);
            newDie.Type = die;
            newDie.Roll();
            dice.Add(newDie);
        }
    }

    private void DestroyDice()
    {
        foreach (var die in dice)
        {
            if (die != null) Destroy(die.gameObject);
        }
    }

    public void RemoveDice(List<Die> diceToRemove)
    {
        dice.RemoveAll(d => diceToRemove.Contains(d));
    }

    public void GainPower(int amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Min(currentBattery, MaxBattery);

        if (currentBattery == MaxBattery)
        {
            BatteryFull();
        }
    }

    public void DrawPower(int amount)
    {
        currentBattery -= amount;
        currentBattery = Mathf.Max(currentBattery, 0);

        if (currentBattery == 0)
        {
            BatteryEmpty();
        }
    }

    protected int SumDice(List<Die> diceList)
    {
        return diceList.Sum(d => d.CurrentValue);
    }
    protected List<Die> GetDiceFromTransform(Transform target)
    {
        var list = new List<Die>();

        foreach (Transform child in target)
        {
            var die = child.GetComponent<Die>();
            list.Add(die);
        }

        return list;
    }

    public abstract void EndTurn();
    protected abstract void BatteryFull();
    protected abstract void BatteryEmpty();
}
