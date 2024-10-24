using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileOwnership { Player, Enemy}
public abstract class Projectile : GameBehaviour
{
    public WeaponData weaponData;
    public float damage;
    public float critChance;
    public float critMultiplier;
    public float duration;
    public float velocity;
    public ProjectileOwnership ownership;
    public bool isPiercing;

    private void OnTriggerEnter(Collider other)
    {
        switch (ownership)
        {
            case ProjectileOwnership.Player:
                if (other.CompareTag("Enemy"))
                {
                    Enemy enemy = other.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        float critRoll = Random.value;
                        if (critChance > critRoll)
                        {
                            enemy.TakeDamage(damage * critMultiplier, true);
                            Debug.Log("Crit for " + damage * critMultiplier);
                        }
                        else
                        {
                            enemy.TakeDamage(damage, false);
                        }
                        OnHit();

                        //_PR.CallOnHitUpdate(enemy);
                    }
                }

                if (other.CompareTag("EnemyBullet"))
                {
                    Enemy enemy = other.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        OnHit();
                    }
                }
                break;
        }
    }

    private void OnHit()
    {
        print("Projectile is hitting");
        if (isPiercing)
        {
            //Play particle
            print("is piercing");
            return;
        }
        else
        {
            //Play particle
            print("is not piercing and atempting to destroy");
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
