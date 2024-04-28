using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySelectionCanvas : MonoBehaviour
{
    public List<Enemy_SO> Enemies;
    public EnemySelectionCharacter EnemySelectionCharacterPrefab;
    public RectTransform EnemySelectionCharacterParent;

    public static int SelectedEnemy;

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
        SelectedEnemy = index;
        SceneManager.LoadScene(2);
    }

    public void OnExit()
    {
        SceneManager.LoadScene(0);
    }
}
