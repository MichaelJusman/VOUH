using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmo : GameBehaviour
{
    [SerializeField] private FloatVariable _ammoMax;
    [SerializeField] private FloatVariable _ammoCurrent;
    [SerializeField] private FloatVariable _ammoMult;

    private void Start()
    {
        _ammoCurrent.Value = _ammoMax;
        _ammoCurrent.OnValueChanged += OnAmmoChanged;
        _ammoCurrent.MinMax = new Vector2(0, _ammoMax);
    }

    private void OnDestroy()
    {
        _ammoCurrent.OnValueChanged -= OnAmmoChanged;
    }

    private void OnAmmoChanged(float newValue)
    {
        var diff = newValue - _ammoCurrent;

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
