using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : GameBehaviour
{
    [SerializeField] private ScriptableListEnemy _scriptableListEnemy;
    [SerializeField] private Pickup[] pickups;

    private void Awake()
    {
        _scriptableListEnemy.OnItemRemoved += OnEnemyDied;
    }

    private void OnDestroy()
    {
        _scriptableListEnemy.OnItemRemoved -= OnEnemyDied;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        Instantiate(pickups[Random.Range(0, pickups.Length)], enemy.transform.position, Quaternion.identity);

    }
}
