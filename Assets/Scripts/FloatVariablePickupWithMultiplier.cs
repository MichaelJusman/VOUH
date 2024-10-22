using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatVariablePickupWithMultiplier : Pickup
{
    [SerializeField] private FloatVariable _floatVariable;
    [SerializeField] private FloatVariable _value;
    [SerializeField] private FloatVariable _multiplier;

    protected override void OnTriggerEnter(Collider other)
    {
        float RoundedVal = Mathf.Round(_value * _multiplier);
        
        _floatVariable.Add(RoundedVal);
        base.OnTriggerEnter(other);
    }
}
