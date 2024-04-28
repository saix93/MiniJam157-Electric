using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public Image Border;
    public GridLayoutGroup DiceBox;
    public DieSlot DieSlotPrefab;
    public RectTransform Description;
    public AbilityEffectUI AbilityEffectPrefab;
    public Image ButtonImage;
    public Color FullColor;
    public Color RegularColor;
    
    private Ability_SO config;
    private GameManager gm;

    public List<DieSlot> Slots { get; private set; }
    private List<Die> dice => Slots.Select(s => s.CurrentDie).ToList();

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        Slots = new List<DieSlot>();
    }

    private void Update()
    {
        ButtonImage.color = IsFull() ? FullColor : RegularColor;
    }

    public void Init(Ability_SO newConfig)
    {
        config = newConfig;

        Title.text = config.Name;
        Border.color = config.AbilityColor;

        foreach (Transform child in DiceBox.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in Description.transform)
        {
            Destroy(child.gameObject);
        }

        Slots.Clear();
        foreach (var die in config.Dice)
        {
            var dieSlot = Instantiate(DieSlotPrefab, DiceBox.transform);
            dieSlot.Init(die);
            Slots.Add(dieSlot);
        }

        foreach (var effect in config.Effects)
        {
            var effectLabel = Instantiate(AbilityEffectPrefab, Description);
            var effectConfig = GameManager.Instance.GetEffectConfig(effect);
            effectLabel.Init(effectConfig, effect);
        }
    }

    public void UseAbilityPlayer()
    {
        UseAbility(GameManager.Instance.Player, GameManager.Instance.Enemy);
    }

    public void UseAbilityEnemy()
    {
        UseAbility(GameManager.Instance.Enemy, GameManager.Instance.Player);
    }

    private void UseAbility(Character user, Character enemy)
    {
        if (!IsFull()) return;
        
        // TODO: Resolver efectos
        foreach (var effect in config.Effects)
        {
            var val = effect.Value;
            switch (effect.Type)
            {
                case AbilityEffectType.TransferEnergy:
                    user.DrawPower(val);
                    enemy.GainPower(val);
                    break;
                case AbilityEffectType.GenerateEnergy:
                    user.GainPower(val);
                    break;
                case AbilityEffectType.WasteEnergy:
                    enemy.DrawPower(val);
                    break;
                case AbilityEffectType.AddResistance:
                    user.AddResistance(val);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        Destroy(gameObject);
        user.RemoveDice(dice);
    }

    public List<int> GetDiceCombination(List<Die> dicePool)
    {
        var slotPossibilities = new Dictionary<int, List<int>>();

        for (var i = 0; i < Slots.Count; i++)
        {
            var slot = Slots[i];
            var possibleDice = dicePool.FindAll(d => slot.CheckRequirements(d));
            slotPossibilities[i] = possibleDice.Select(d => dicePool.IndexOf(d)).ToList();
        }

        if (HasEmptyPossibilities(slotPossibilities))
        {
            return new List<int>();
        }
        
        var combination = HasUniqueCombination(slotPossibilities);
        return combination;
    }

    #region Unique combinations
    private List<int> HasUniqueCombination(Dictionary<int, List<int>> dictionary)
    {
        var keys = dictionary.Keys.ToArray();
        var combinations = new List<List<int>>();
        GenerateCombinations(dictionary, keys, new List<int>(), combinations);
        foreach (var combination in combinations)
        {
            if (combination.Distinct().Count() == combination.Count)
            {
                return combination;
            }
        }
        return new List<int>(); // Return an empty list if no unique combination is found
    }

    private void GenerateCombinations(Dictionary<int, List<int>> dictionary, int[] keys, List<int> current, List<List<int>> combinations)
    {
        if (current.Count == keys.Length)
        {
            combinations.Add(new List<int>(current));
            return;
        }

        var currentKey = keys[current.Count];
        foreach (var value in dictionary[currentKey])
        {
            current.Add(value);
            GenerateCombinations(dictionary, keys, current, combinations);
            current.RemoveAt(current.Count - 1);
        }
    }

    private bool HasEmptyPossibilities(Dictionary<int, List<int>> dictionary)
    {
        foreach (var pair in dictionary)
        {
            if (pair.Value.Count == 0) return true;
        }

        return false;
    }
    #endregion

    private bool IsFull()
    {
        return Slots.All(s => s.HasDie);
    }
}
