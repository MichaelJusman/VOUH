using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : GameBehaviour
{
    [SerializeField] private FloatVariable _energyMax;
    [SerializeField] private FloatVariable _energyCurrent;
    [SerializeField] private FloatVariable _energyRegen;

    private void Start()
    {
        _energyCurrent.Value = _energyMax;
        _energyCurrent.OnValueChanged += OnEnergyChanged;
    }

    private void OnDestroy()
    {
        _energyCurrent.OnValueChanged -= OnEnergyChanged;
    }

    private void OnEnergyChanged(float newValue)
    {
        var diff = newValue - _energyCurrent;

        if (diff < 0)
        {
            //Damaged Logic
        }
        else
        {
            //Healing Logic
        }
    }
}
