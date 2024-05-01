using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ME_Weapon : MonoBehaviour
{
    public string weaponName = "";
    public float fireRate = 1f;
    public int projectileCount = 2;

    private float lastShotTime;

    // transferred stats 
    public float damage = 0;
    public float speed = 1f;
    public int lifespan = 2;

    public virtual void Shoot()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            SpawnProjectiles();
            lastShotTime = Time.time;
        }
    }

    public virtual void SpawnProjectiles()
    {

    }

    public virtual GameObject BuildProjectile(GameObject projectilePrefab)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        ME_Projectile bullet = projectile.GetComponent<ME_Projectile>();

        bullet.damage = damage;
        bullet.speed = speed;
        bullet.lifespan = lifespan;

        return projectile;
    }

    public virtual GameObject BuildProjectileStatic(GameObject projectilePrefab)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.transform.SetParent(this.transform);
        ME_ProjectileStatic sword = projectile.GetComponent<ME_ProjectileStatic>();

        sword.damage = damage;
        sword.speed = speed;

        return projectile;
    }
}
