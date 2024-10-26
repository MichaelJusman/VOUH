using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class Rifle : Weapon
{
    [Header("Weapon Inputs")]
    [SerializeField] private BoolVariable _primaryInput;
    [SerializeField] private BoolVariable _altInputInput;

    protected override void FiringBehaviour()
    {
        if (_primaryInput) { PrimaryFire(); }
        if (_altInputInput) { AlternativeFire(); }
    }

    protected override void PrimaryFire()
    {
        if(lastFireTime >= weaponData.fireRate && currentClip > 0)
        {
            Shoot();
        }
    }

    protected override void StateLogic()
    {
        if(isEquiped)
        {
            weaponState = WeaponState.Ready;

            if(lastFireTime <= weaponData.fireRate)
            {
                weaponState = WeaponState.Gap;
            }
            else if(_reloadInput && currentClip < weaponData.clipsize)
            {
                weaponState = WeaponState.Reloading;
            }
        }
        else
        {
            weaponState = WeaponState.Unequiped;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(mainProjectile, firingPoint.position, firingPoint.rotation);

        if (bullet.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = firingPoint.up * weaponData.projectileSpeed;
        }

        if (bullet.TryGetComponent<Projectile>(out Projectile projectile))
        {
            projectile.damage = weaponData.damage;
            projectile.critChance = weaponData.critChance;
            projectile.critMultiplier = weaponData.critMultiplier;
            projectile.duration = weaponData.projectileDuration;
        }
    }
}
