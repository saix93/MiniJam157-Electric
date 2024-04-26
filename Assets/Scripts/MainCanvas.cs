using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public TextMeshProUGUI CurrentVoltsText;
    public TextMeshProUGUI CurrentAmpsText;
    
    public TextMeshProUGUI PredictedVoltsText;
    public TextMeshProUGUI PredictedAmpsText;

    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        CurrentVoltsText.text = $"Current Volts: {gm.CurrentVolts:F1}{gm.Measurements.Volts}";
        CurrentAmpsText.text = $"Current Amps: {gm.CurrentAmps:F1}{gm.Measurements.Amps}";
        
        PredictedVoltsText.text = $"Predicted Volts: {gm.PredictedVolts:F1}{gm.Measurements.Volts}";
        PredictedAmpsText.text = $"Predicted Amps: {gm.PredictedAmps:F1}{gm.Measurements.Amps}";
    }
}
