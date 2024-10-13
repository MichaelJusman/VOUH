using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : GameBehaviour
{
    [SerializeField] private Vector3Variable _playerPosotion;

    private void Update()
    {
        _playerPosotion.Value = transform.position;
    }
}
