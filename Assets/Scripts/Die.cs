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
    public List<Sprite> Sprites;
    public int Sides = 6;

    private GameManager gm;
    private DraggableElement drag;
    
    public int CurrentValue { get; private set; }
    public DieType Type { get; set; }

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        drag = GetComponent<DraggableElement>();
    }

    private void Start()
    {
        Image.color = gm.GetDieColor(Type);
    }

    public void OnClick()
    {
        ResetToOriginalParent();
    }

    public void ResetToOriginalParent()
    {
        drag.ResetToOriginalParent();
    }

    public void Roll()
    {
        CurrentValue = Random.Range(1, Sides + 1);
        Image.sprite = Sprites[CurrentValue - 1];
    }
}

public enum DieType
{
    Regular,
    Volts,
    Amps,
}
