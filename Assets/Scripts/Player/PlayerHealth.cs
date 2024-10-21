using Obvious.Soap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHealth : GameBehaviour
{
    [SerializeField] private FloatVariable _healthMax;
    [SerializeField] private FloatVariable _healthCurrent;
    [SerializeField] private FloatVariable _healthRegen;
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private float regentimer = 0;

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

    private void Update()
    {
        if(_healthCurrent < _healthMax && !isDead)
        {
            HealthRegen();
            
        }
    }

    void HealthRegen()
    {
        if(regentimer >= regenInterval)
        {
            _healthCurrent.Value += _healthRegen;
            regentimer = 0;
        }
        else
        {
            regentimer += Time.deltaTime;
        }
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
            _onPlayerHealed.Raise(Mathf.RoundToInt(diff));
        }
    }

    public void OnDamaged(int val)
    {
        if(!isInvulnerable)
        {
            if (_healthCurrent <= 0 && !isDead)
            {
                _onPlayerDeath.Raise();
            }
            else
            {
                _onPlayerDamaged.Raise(Mathf.RoundToInt(DamageCalculation(val)));
            }
        }
        
    }

    float DamageCalculation(float damage)
    {
        Debug.Log("Damage before armor reduction is " + damage);
        float reduction = (damage * Mathf.Pow((1 - armorMultiplier), _armor));
        float damageTaken = Mathf.RoundToInt(reduction);
        Debug.Log("Damage after armor reduction is " + reduction + " rounded as " + damageTaken);
        float reducedDamage = Mathf.RoundToInt(damage - damageTaken);
        Debug.Log("Damage reduced is " + reducedDamage);
        if(reducedDamage > 0)
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
            _healthCurrent.Add(heal);
            
        }
        
    }

    void OnDeath()
    {
        isDead = true;
    }

    public void TakeDmage(int val)
    {
        _healthCurrent.Add(- val);
    }


}
