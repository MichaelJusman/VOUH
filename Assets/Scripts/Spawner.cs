using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : GameBehaviour
{
    [Header("Spawning Logic")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector3Variable _playerPosition;
    [SerializeField] private Vector2 spawnRange;
    private float _currentAngle;

    [Header("Timer")]
    [SerializeField] private FloatVariable _spawnInterval;
    [SerializeField] private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= _spawnInterval)
        {
            Spawn();
            timer = 0;
        }
    }

    void Spawn()
    {
        _currentAngle += 180 + Random.Range(-45, 45);
        var angleRadius = _currentAngle * Mathf.Deg2Rad;
        var range = Random.Range(spawnRange.x, spawnRange.y);
        var relativePos = new Vector3(Mathf.Cos(angleRadius)* range, 0f, Mathf.Sin(angleRadius)* range);
        var spawnPos = _playerPosition.Value + relativePos;
        Instantiate(prefab, spawnPos, Quaternion.identity, transform);
    }
}
