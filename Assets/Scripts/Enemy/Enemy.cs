using Obvious.Soap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : GameBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] private FloatVariable _roundMultiplier;

    [Header("Events")]
    [SerializeField] private ScriptableEventInt _onEnemyHitPlayer;

    [Header("Player References")]
    [SerializeField] private Vector3Variable _playerPos;
    [SerializeField] private FloatVariable _retaliate;
    protected Vector3 direction;

    private void Update()
    {
        UpdateDirection();
        EnemyBehaviour();
    }

    protected abstract void EnemyBehaviour();

    void UpdateDirection()
    {
        direction = (_playerPos.Value - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enemy collided with something");
        if (other.CompareTag("Player"))
        {
            Debug.Log("player collided with enemy");
            _onEnemyHitPlayer.Raise((int)damage);
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        health -= _retaliate.Value;

        if (health > 0)
        {
            //Call the number text spawner
        }
        else
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}
