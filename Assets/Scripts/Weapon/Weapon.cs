using Obvious.Soap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Health, Energy, Mana, Ammo, Default }
public abstract class Weapon : GameBehaviour
{
    [Header("Weapon Info")]
    public WeaponData weaponData;
    public int currentClip;
    public bool isReloading = false;
    public GameObject mainProjectile;
    public GameObject secondary;

    [Header("Weapon Leveling")]
    public int currentLevel;
    public int killCount;
    public int killTreshold;
    public float killMultiplier;

    [Header("Weapon Inputs")]
    [SerializeField] private BoolVariable _primaryInput;
    [SerializeField] private BoolVariable _altInputInput;

    [Header("Resources Reference")]
    [SerializeField] private FloatVariable _health;
    [SerializeField] private FloatVariable _energy;
    [SerializeField] private FloatVariable _mana;
    [SerializeField] private FloatVariable _ammo;

    private void Update()
    {
        if(!isReloading && currentClip > 0)
        {
            if(_primaryInput) { PrimaryFire(); }
            if(_altInputInput) { AlternativeFire(); }
        }
    }

    protected abstract void PrimaryFire();
    protected abstract void AlternativeFire();

    protected virtual void Reload()
    {
        if (currentClip < weaponData.clipsize)
        {
            if (weaponData.weaponType == WeaponType.Default)
            {
                isReloading = true;
                StartCoroutine(Reloading());
                return; // Skip resource checks
            }

            float resourceAmount = 0;
            Action<float> deductResource = null;

            // Assign the correct resource and deduction method
            switch (weaponData.weaponType)
            {
                case WeaponType.Health:
                    resourceAmount = _health.Value;
                    deductResource = (amount) => _health.Add(amount);
                    break;
                case WeaponType.Energy:
                    resourceAmount = _energy.Value;
                    deductResource = (amount) => _energy.Add(amount);
                    break;
                case WeaponType.Mana:
                    resourceAmount = _mana.Value;
                    deductResource = (amount) => _mana.Add(amount);
                    break;
                case WeaponType.Ammo:
                    resourceAmount = _ammo.Value;
                    deductResource = (amount) => _ammo.Add(amount);
                    break;

                case WeaponType.Default:
                    break;
            }

            // Check if enough resources are available
            float totalCost = weaponData.bulletCost * weaponData.clipsize;
            if (totalCost <= resourceAmount)
            {
                isReloading = true;
                deductResource(-totalCost);
                StartCoroutine(Reloading());
            }
        }
    }

    protected virtual IEnumerator Reloading()
    {
        yield return new WaitForSeconds(weaponData.reloadTime);

        currentClip = weaponData.clipsize;
        isReloading = false;
    }
}
