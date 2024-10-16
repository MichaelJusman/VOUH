using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ScriptableEventInt _onEnemyHitPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player collided with enemy");
            _onEnemyHitPlayer.Raise(5);
        }
    }
}
