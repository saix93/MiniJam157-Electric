using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectionCharacter : MonoBehaviour
{
    public TextMeshProUGUI NameLabel;
    public Image Image;
    public Button Button;

    public void Init(Enemy_SO config, int index, Action<int> onClick)
    {
        NameLabel.text = config.Name;
        Image.sprite = config.Sprite;

        Button.onClick.AddListener(() => onClick(index));
    }
}
