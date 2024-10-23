using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Health, Energy, Mana, Ammo }
public abstract class Weapon : GameBehaviour
{
    [Header("Weapon Info")]
    [SerializeField] private StringVariable weaponName;
    [SerializeField] private StringVariable weaponDescription;

    [Header("Weapon Stats")]
    [SerializeField] private FloatVariable weaponLevel;
    [SerializeField] private FloatVariable weaponDamage;
    [SerializeField] private FloatVariable weaponFirerate;
    [SerializeField] private WeaponType weaponType;
}
