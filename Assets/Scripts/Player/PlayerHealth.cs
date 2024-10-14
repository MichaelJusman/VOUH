using Obvious.Soap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : GameBehaviour
{
    [SerializeField] private FloatVariable _healthMax;
    [SerializeField] private FloatVariable _healthCurrent;
    [SerializeField] private FloatVariable _healthRegen;

    private void Start()
    {
        _healthCurrent.Value = _healthMax;
        _healthCurrent.OnValueChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        _healthCurrent.OnValueChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float newValue)
    {
        var diff = newValue - _healthCurrent;

        if(diff < 0)
        {
            //Damaged Logic
        }
        else
        {
            //Healing Logic
        }
    }
}
