using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        
    }
}
