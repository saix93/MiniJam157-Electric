using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    public float TimeBeforeAssignDice = 2f;
    public float TimeBeforePlayingAbilities = 2f;
    public float TimeToPlayEachDie = 1f;
    public float TimeAfterPlayingAbilities = 2f;
    public List<Enemy_SO> Configs;

    private Enemy_SO currentConfig;

    protected override void Awake()
    {
        base.Awake();

        foreach (Transform child in DiceParent)
        {
            Destroy(child.gameObject);
        }
    }

    protected override void Start()
    {
        currentConfig = Configs[EnemySelectionCanvas.SelectedEnemy];

        GameManager.Instance.MaxTurns = currentConfig.MaxTurns;
        
        maxBattery = currentConfig.BatteryCapacity;
        currentBattery = currentConfig.InitialBattery;
        dicePool = currentConfig.DiceConfig;
        abilities = new List<Ability_SO>(currentConfig.Abilities);
        CharacterImage.sprite = currentConfig.Sprite;

        NameLabel.text = currentConfig.Name;
        
        base.Start();
    }

    public override void StartTurn()
    {
        base.StartTurn();

        StartCoroutine(PlayTurnCR());
    }

    private IEnumerator PlayTurnCR()
    {
        var shuffledAbilities = GameManager.Instance.GetShuffledList(currentAbilities);
        foreach (var ability in shuffledAbilities)
        {
            yield return new WaitForSeconds(TimeBeforeAssignDice);
            
            var combination = ability.GetDiceCombination(dice);
            if (combination.Count > 0)
            {
                var usedDice = combination.Select(index => dice[index]).ToList();
                for (var i = 0; i < usedDice.Count; i++)
                {
                    var die = usedDice[i];
                    yield return StartCoroutine(MoveDieCR(die, ability.Slots[i]));
                }
            
                yield return new WaitForSeconds(TimeBeforePlayingAbilities);
            
                ability.UseAbilityEnemy();
            }
        }
        
        yield return new WaitForSeconds(TimeAfterPlayingAbilities);
        
        EndTurn();
    }

    private IEnumerator MoveDieCR(Die die, DieSlot slot)
    {
        die.transform.SetParent(die.transform.root);
        var originalDiePos = die.transform.position;
        var dieDestination = slot.transform.position;

        var t = 0f;

        while (t < 1f)
        {
            die.transform.position = Vector3.Lerp(originalDiePos, dieDestination, t);
            
            t += Time.deltaTime / TimeToPlayEachDie;
            yield return null;
        }
        
        die.transform.SetParent(slot.transform);
    }

    public override void EndTurn()
    {
        // var usedDice = GetDiceFromTransform(DiceParent);
        //
        // var usedAmps = SumDice(usedDice.FindAll(d => d.Type == DieType.Amps));
        // var usedVolts = SumDice(usedDice.FindAll(d => d.Type == DieType.Volts));
        
        gm.EndEnemyTurn();
    }

    protected override void BatteryFull()
    {
        gm.EnemyEliminated();
    }

    protected override void BatteryEmpty()
    {
        
    }
}
