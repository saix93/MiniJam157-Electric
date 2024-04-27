using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    public Image BatteryImage;
    public TextMeshProUGUI BatteryText;
    public RectTransform DiceParent;
    public int MaxBattery = 100;
    public DiceConfig_SO DiceConfig;
    public List<Ability_SO> Abilities;

    [Header("Parameters")]
    public int DefaultAttackAmps = 2;
    public int DefaultAttackVolts = 1;

    private List<DieType> dicePool;

    protected List<Die> dice;
    protected GameManager gm;
    
    private float batteryPercentage => (float)currentBattery / (float)MaxBattery;
    protected int currentBattery { get; set; }

    protected virtual void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        dicePool = new List<DieType>(DiceConfig.Dice);
        dice = new List<Die>();
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
        
        foreach (var ability in Abilities)
        {
            var newAbility = Instantiate(gm.AbilityPrefab, gm.AbilitiesParent);
            newAbility.Init(ability);
        }
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
            Destroy(die.gameObject);
        }
    }

    public void GainPower(int amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Min(currentBattery, MaxBattery);
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
    protected abstract void BatteryEmpty();
}
