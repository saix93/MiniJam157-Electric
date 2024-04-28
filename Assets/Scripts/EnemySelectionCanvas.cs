using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectionCanvas : MonoBehaviour
{
    public List<Enemy_SO> Enemies;
    public EnemySelectionCharacter EnemySelectionCharacterPrefab;
    public RectTransform EnemySelectionCharacterParent;

    private void Awake()
    {
        foreach (Transform child in EnemySelectionCharacterParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (var i = 0; i < Enemies.Count; i++)
        {
            var enemy = Enemies[i];
            var character = Instantiate(EnemySelectionCharacterPrefab, EnemySelectionCharacterParent);
            character.Init(enemy, i, OnClick);
        }
    }

    private void OnClick(int index)
    {
        Debug.Log($"Clicked on {index}");
    }
}
