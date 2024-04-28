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
    public GameObject ResistanceIcon;
    public TextMeshProUGUI ResistanceLabel;
    public TextMeshProUGUI NameLabel;
    public Image BatteryImage;
    public TextMeshProUGUI BatteryText;
    public RectTransform DiceParent;
    public Image CharacterImage;

    [Header("Parameters")]
    public int MaxAbilities = 4;

    private List<DieType> currentDice;

    protected int maxBattery = 100;
    protected List<DieType> dicePool;
    protected List<Die> dice;
    protected List<Ability_SO> abilities;
    protected List<Ability> currentAbilities;
    protected GameManager gm;
    
    private float batteryPercentage => (float)currentBattery / (float)maxBattery;
    protected int currentBattery { get; set; }
    protected int currentResistance { get; set; }

    protected virtual void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        dice = new List<Die>();
        currentAbilities = new List<Ability>();
    }

    protected virtual void Start()
    {
        currentDice = new List<DieType>(dicePool);
    }

    private void Update()
    {
        BatteryImage.fillAmount = batteryPercentage;
        BatteryText.text = $"{currentBattery}/{maxBattery}";
        
        ResistanceIcon.SetActive(currentResistance > 0);
        ResistanceLabel.text = $"{currentResistance}";
    }
    
    public virtual void StartTurn()
    {
        currentResistance = 0;
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
        
        var randomAbilities = GetRandomElements(abilities, MaxAbilities);
        
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

        foreach (var die in currentDice)
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
        amount = AbsorbResistance(amount);
        
        currentBattery += amount;
        currentBattery = Mathf.Min(currentBattery, maxBattery);

        if (currentBattery == maxBattery)
        {
            BatteryFull();
        }
    }

    public void DrawPower(int amount)
    {
        amount = AbsorbResistance(amount);
        
        currentBattery -= amount;
        currentBattery = Mathf.Max(currentBattery, 0);

        if (currentBattery == 0)
        {
            BatteryEmpty();
        }
    }

    private int AbsorbResistance(int amount)
    {
        if (currentResistance > 0)
        {
            if (currentResistance >= amount)
            {
                currentResistance -= amount;
                amount = 0;
            }
            else
            {
                amount -= currentResistance;
                currentResistance = 0;
            }
        }

        return amount;
    }

    public void AddResistance(int amount)
    {
        currentResistance += amount;
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
