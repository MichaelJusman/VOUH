using Obvious.Soap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : GameBehaviour
{
    [SerializeField] private FloatVariable _manaMax;
    [SerializeField] private FloatVariable _manaCurrent;
    [SerializeField] private FloatVariable _manaRegen;
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private float regentimer = 0;

    private void Start()
    {
        _manaCurrent.Value = _manaMax;
        _manaCurrent.OnValueChanged += OnManaChanged;
    }

    private void OnDestroy()
    {
        _manaCurrent.OnValueChanged -= OnManaChanged;
    }

    private void Update()
    {
        if (_manaCurrent < _manaMax)
        {
            ManaRegen();
        }
    }

    private void ManaRegen()
    {
        if (regentimer >= regenInterval)
        {
            _manaCurrent.Value += _manaRegen;
            regentimer = 0;
        }
        else
        {
            regentimer += Time.deltaTime;
        }
    }

    private void OnManaChanged(float newValue)
    {
        var diff = newValue - _manaCurrent;

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
