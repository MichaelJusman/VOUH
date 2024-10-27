using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public WeaponData[] weaponSlots = new WeaponData[3];  // Slot 1, 2, 3
    private int currentWeaponIndex = 0;

    [SerializeField] private Collider pickupCollider;
    [SerializeField] private Transform weaponTransform;

    [SerializeField] private WeaponDrop pendingWeapon;

    void Update()
    {
        HandleWeaponSwitching();
        HandleFiring();
    }
    
    void HandleWeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentWeaponIndex = 0; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { currentWeaponIndex = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { currentWeaponIndex = 2; }

        // Mouse wheel weapon cycling
        if (Input.GetAxis("Mouse ScrollWheel") > 0) { CycleWeapons(1); }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) { CycleWeapons(-1); }
    }

    void CycleWeapons(int direction)
    {
        currentWeaponIndex = (currentWeaponIndex + direction + weaponSlots.Length) % weaponSlots.Length;
    }

    void HandleFiring()
    {
        if (Input.GetMouseButtonDown(0)) { FirePrimary(); }
        if (Input.GetMouseButtonDown(1)) { FireSecondary(); }
    }

    private void OnTriggerStay(Collider other)
    {
        //Detect collision with weapon drop, put into PendingWeapon, when pressing F, equip the weapon: (if slot 1 and 2 is empty, fill slot 1, if slot 1 is filled, fill slot 2. If both slots are filled, replace currently equiped weapon slot, except for weapon slot 3, where the default weapon exist and cannot be replace. Incase of picking up weapon while on slot 3 with slot 1 & 2 filled, swap slot 1.
    }


    void FirePrimary()
    {
        // Use currentWeaponIndex to fire the selected weapon
        WeaponData weapon = weaponSlots[currentWeaponIndex];
        // Fire logic based on weapon.fireRate, damage, etc.
    }

    void FireSecondary()
    {
        // Handle alt-fire if the weapon has a secondary fire mode
        WeaponData weapon = weaponSlots[currentWeaponIndex];
        // Alt-fire logic based on weapon's secondary fire
    }
}
