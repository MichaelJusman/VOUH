using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : GameBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private FloatVariable _roundMultiplier;

    [Header("Events")]
    [SerializeField] private ScriptableEventInt _onEnemyHitPlayer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enemy collided with something");
        if (other.CompareTag("Player"))
        {
            Debug.Log("player collided with enemy");
            _onEnemyHitPlayer.Raise((int)damage);
        }
    }
}
