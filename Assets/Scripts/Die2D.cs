using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Die2D : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI ButtonText;
    public Image ButtonImage;
    public Color BlockedColor = Color.red;
    public Color UnblockedColor = Color.white;

    private GameManager gm;
    
    public bool IsBlocked { get; private set; }
    public DieSide CurrentSide { get; private set; }
    private List<DieSide> sides { get; set; }

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        ButtonImage.color = IsBlocked ? BlockedColor : UnblockedColor;
    }

    public void LoadConfig(DieConfig_SO config)
    {
        sides = new List<DieSide>(config.Sides);
    }

    public void BlockButton()
    {
        IsBlocked = !IsBlocked;
    }

    public void Clear()
    {
        CurrentSide = null;
        IsBlocked = false;
        
        gameObject.SetActive(false);
    }

    public void Roll()
    {
        if (sides.Count == 0)
        {
            return;
        }

        var sideIndex = Random.Range(0, sides.Count);
        var side = sides[sideIndex];
        
        SetDieText(sideIndex, side);

        CurrentSide = side;
    }

    private void SetDieText(int sideIndex, DieSide side)
    {
        ButtonText.text = $"D{sides.Count} ({sideIndex})";
        foreach (var effect in side.Effects)
        {
            ButtonText.text += $"\n";
            
            switch (effect.Type)
            {
                case DieSideEffectType.None:
                    ButtonText.text += $"None";
                    break;
                case DieSideEffectType.Volts:
                    ButtonText.text += $"{effect.Value}{gm.Measurements.Volts}";
                    break;
                case DieSideEffectType.Amps:
                    ButtonText.text += $"{effect.Value}{gm.Measurements.Amps}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
