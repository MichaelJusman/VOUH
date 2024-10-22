using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatVariablePickup : Pickup
{
    [SerializeField] private FloatVariable _floatVariable;
    [SerializeField] private FloatVariable _value;
    
    protected override void OnTriggerEnter(Collider other)
    {
        _floatVariable.Add(_value);
        base.OnTriggerEnter(other);
    }
}
