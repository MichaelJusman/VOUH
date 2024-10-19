using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameBehaviour
{
    [SerializeField] private ScriptableEventInt _onEnemyHitPlayer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enemy collided with something");
        if (other.CompareTag("Player"))
        {
            Debug.Log("player collided with enemy");
            _onEnemyHitPlayer.Raise(5);
        }
    }
}
