using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : GameBehaviour
{
    [SerializeField] private FloatVariable _manaMax;
    [SerializeField] private FloatVariable _manaCurrent;
    [SerializeField] private FloatVariable _manaRegen;

    private void Start()
    {
        _manaCurrent.Value = _manaMax;
        _manaCurrent.OnValueChanged += OnManaChanged;
    }

    private void OnDestroy()
    {
        _manaCurrent.OnValueChanged -= OnManaChanged;
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
