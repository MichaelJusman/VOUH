using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public string weaponDescription;

    [Header("Weapon Stats")]
    public float damage;
    public float fireRate;
    public float critMultiplier;
    public float critChance;
    public int clipsize;

    [Header("Projectile Stats")]
    public int bulletCost;
    public float reloadTime;
    public float projectileDuration;
    public float projectileSpeed;

    [Header("Weapon Type")]
    public WeaponType weaponType;
    //Upgade enums here
}
