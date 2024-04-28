using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Character
{
    public float TimeBeforePlayingAbilities = 2f;
    public float TimeToPlayEachDie = 1f;
    public float TimeAfterPlayingAbilities = 2f;
    public Enemy_SO Config;

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
        MaxBattery = Config.BatteryCapacity;
        currentBattery = Config.InitialBattery;
        DiceConfig = Config.DiceConfig;
    }

    public override void StartTurn()
    {
        base.StartTurn();

        StartCoroutine(PlayTurnCR());
    }

    private IEnumerator PlayTurnCR()
    {
        yield return new WaitForSeconds(TimeBeforePlayingAbilities);

        var shuffledAbilities = GameManager.Instance.GetShuffledList(currentAbilities);
        foreach (var ability in shuffledAbilities)
        {
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
        Debug.LogError($"Enemy done!");
    }

    protected override void BatteryEmpty()
    {
        
    }
}
