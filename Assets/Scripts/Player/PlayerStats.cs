using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private FloatVariable _Vigor;
    [SerializeField] private FloatVariable _Tenacity;
    [SerializeField] private FloatVariable _Blessing;
    [SerializeField] private FloatVariable _Freedom;
    [SerializeField] private FloatVariable _Audacity;

    [Header("Primary Stats")]
    [SerializeField] private FloatVariable _maxHealth;
    [SerializeField] private FloatVariable _healthRegen;
    [SerializeField] private FloatVariable _maxEnergy;
    [SerializeField] private FloatVariable _energyRegen;
    [SerializeField] private FloatVariable _maxMana;
    [SerializeField] private FloatVariable _manaRegen;
    [SerializeField] private FloatVariable _maxAmmo;
    [SerializeField] private FloatVariable _ammoMultiplier;
    [SerializeField] private FloatVariable _retaliate;
    [SerializeField] private FloatVariable _armor;

    [Header("Secondary Stats")]
    [SerializeField] private FloatVariable _vampirism;
    [SerializeField] private FloatVariable _topSpeed;
    [SerializeField] private FloatVariable _leech;
    [SerializeField] private FloatVariable _magnetRadius;
    [SerializeField] private FloatVariable _shieldKnockback;

    [Header("Stat Multiplier")]
    [SerializeField] private FloatVariable _maxHealthPerVigor;
    [SerializeField] private FloatVariable _healthRegenPerVigor;
    [SerializeField] private FloatVariable _vampirismPerVigor;
    [SerializeField] private FloatVariable _maxEnergyPerTenacity;
    [SerializeField] private FloatVariable _energyRegenPerTenacity;
    [SerializeField] private FloatVariable _topSpeedPerTenacity;
    [SerializeField] private FloatVariable _maxManaPerBlessing;
    [SerializeField] private FloatVariable _manaRegenPerBlessing;
    [SerializeField] private FloatVariable _leechPerBlessing;
    [SerializeField] private FloatVariable _maxAmmoPerFreedom;
    [SerializeField] private FloatVariable _ammoMultiplierPerFreedom;
    [SerializeField] private FloatVariable _magnetRadiusPerFreedom;
    [SerializeField] private FloatVariable _retaliatePerAudacity;
    [SerializeField] private FloatVariable _armorPerAudacity;
    [SerializeField] private FloatVariable _shieldKnockbackPerAudacity;

    [Header("Current Resources")]
    [SerializeField] private FloatVariable _currentHealth;
    [SerializeField] private FloatVariable _currentEnergy;
    [SerializeField] private FloatVariable _currentMana;
    [SerializeField] private FloatVariable _currentAmmo;


    private void Start()
    {
        _Vigor.OnValueChanged += OnVigorGain;
        _Tenacity.OnValueChanged += OnTenacityGain;
        _Blessing.OnValueChanged += OnBlessingGain;
        _Freedom.OnValueChanged += OnFreedomGain;
        _Audacity.OnValueChanged += OnAudacityGain;
    }

    void OnVigorGain(float newVal)
    {
        _maxHealth.Value += _maxHealthPerVigor;
        _healthRegen.Value += _healthRegenPerVigor;

        if(_Vigor.Value % 5 == 0)
        {
            _vampirism.Value += _vampirismPerVigor;
        }
    }

    void OnTenacityGain(float newVal)
    {
        _maxEnergy.Value += _maxEnergyPerTenacity;
        _energyRegen.Value += _energyRegenPerTenacity;

        if(_Tenacity.Value % 5 == 0)
        {
            _topSpeed.Value += _topSpeedPerTenacity;
        }
    }

    void OnBlessingGain(float newVal)
    {
        _maxMana.Value += _maxManaPerBlessing;
        _manaRegen.Value += _manaRegenPerBlessing;

        if(_Blessing.Value % 5 == 0)
        {
            _leech.Value += _leechPerBlessing;
        }
    }

    void OnFreedomGain(float newVal)
    {
        _maxAmmo.Value += _maxAmmoPerFreedom;
        _ammoMultiplier.Value += _ammoMultiplierPerFreedom;

        if(_Freedom.Value % 5 == 0)
        {
            _magnetRadius.Value += _magnetRadiusPerFreedom;
        }
    }

    void OnAudacityGain(float newVal)
    {
        _retaliate.Value += _retaliatePerAudacity;
        _armor.Value += _armorPerAudacity;

        if(_Audacity.Value % 5 == 0)
        {
            _shieldKnockback.Value += _shieldKnockbackPerAudacity;
        }
    }
}
