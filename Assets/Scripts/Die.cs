using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Die : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Label;
    public int Sides = 6;

    private GameManager gm;
    
    public int CurrentValue { get; private set; }
    public DieType Type { get; set; }

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        Image.color = gm.GetDieColor(Type);
    }

    private void Update()
    {
        
    }

    public void BlockButton()
    {
        
    }

    public void Roll()
    {
        CurrentValue = Random.Range(1, Sides + 1);
        Label.text = CurrentValue.ToString();
    }
}

public enum DieType
{
    Regular,
    Volts,
    Amps,
}
