using Obvious.Soap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : GameBehaviour
{
    [SerializeField] private FloatVariable _energyMax;
    [SerializeField] private FloatVariable _energyCurrent;
    [SerializeField] private FloatVariable _energyRegen;
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private float regentimer = 0;

    private void Start()
    {
        _energyCurrent.Value = _energyMax;
        _energyCurrent.OnValueChanged += OnEnergyChanged;
    }

    private void OnDestroy()
    {
        _energyCurrent.OnValueChanged -= OnEnergyChanged;
    }

    private void Update()
    {
        if (_energyCurrent < _energyMax)
        {
            EnergyRegen();
        }
    }

    private void EnergyRegen()
    {
        if(regentimer >= regenInterval)
        {
            _energyCurrent.Value += _energyRegen;
            regentimer = 0;
        }
        else { regentimer += Time.deltaTime; }
    }

    private void OnEnergyChanged(float newValue)
    {
        var diff = newValue - _energyCurrent;

        //if (diff < 0)
        //{
        //    _energyCurrent.Add(newValue);
        //}
        //else
        //{
        //    if (_energyCurrent + newValue > _energyMax)
        //    {
        //        _energyCurrent.Value = _energyMax;
        //    }
        //    else
        //    {
        //        _energyCurrent.Add(newValue);
        //    }
        //}
    }
}
