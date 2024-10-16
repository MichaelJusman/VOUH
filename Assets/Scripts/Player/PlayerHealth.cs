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
    [SerializeField] private FloatVariable _armor;
    [SerializeField] private float armorMultiplier;

    [SerializeField] private ScriptableEventInt _onPlayerDamaged;
    [SerializeField] private ScriptableEventInt _onPlayerHealed;
    [SerializeField] private ScriptableEventInt _onPlayerArmorBlock;
    [SerializeField] private ScriptableEventNoParam _onPlayerDeath;
    private bool isDead;
    private bool isInvulnerable;

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
            OnDamaged(Mathf.Abs((int)diff));
        }
        else
        {
            OnHealed(diff);
        }
    }

    public void OnDamaged(int val)
    {
        if(!isInvulnerable)
        {
            if (_healthCurrent <= 0 && !isDead)
            {
                OnDeath();
            }
            else
            {
                _onPlayerDamaged.Raise(Mathf.RoundToInt(DamageCalculation(val)));
            }
        }
        
    }

    float DamageCalculation(float damage)
    {
        float reduction = (damage * Mathf.Pow((1 - armorMultiplier), _armor));
        float roundedReduction = Mathf.Round(reduction);

        float damageTaken = Mathf.RoundToInt(damage - roundedReduction);
        if (damageTaken > 0)
        {
            _onPlayerArmorBlock.Raise((int)damageTaken);
        }

            return damageTaken;
    }

    void OnHealed(float heal)
    {
        if(_healthCurrent + heal > _healthMax)
        {
            _healthCurrent.Value = _healthMax;
        }
        else
        {
            _onPlayerHealed.Raise(Mathf.RoundToInt(heal));
        }
        
    }

    void OnDeath()
    {
        _onPlayerDeath.Raise();
        isDead = true;
    }


}
