using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsSpawner : GameBehaviour
{
    [SerializeField] private ScriptableListEnemy _scriptableListEnemy;
    [SerializeField] private GameObject spawnVFX;
    [SerializeField] private GameObject deathVFX;

    private void Awake()
    {
        _scriptableListEnemy.OnItemAdded += OnEnemySpawned;
        _scriptableListEnemy.OnItemRemoved += OnEnemyDied;
    }

    private void OnDestroy()
    {
        _scriptableListEnemy.OnItemAdded -= OnEnemySpawned;
        _scriptableListEnemy.OnItemRemoved -= OnEnemyDied;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        Instantiate(deathVFX, enemy.transform.position, Quaternion.identity);

    }

    private void OnEnemySpawned(Enemy enemy)
    {
        Instantiate(spawnVFX, enemy.transform.position, Quaternion.identity);
    }
}
