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
    public float damage = 1;
    public float damageFrequency = 0.2f;
    public float speed = 1f;
    public float rotationSpeed = 180f;
    public int lifespan = 2;
    public Vector3 scale = Vector3.one;

    public void Shoot()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            Debug.Log("Shooting!");
            SpawnProjectiles();
            lastShotTime = Time.time;
        }
    }

    public abstract void SpawnProjectiles();

    private GameObject InstantiateProjectile(GameObject projectilePrefab)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        ME_Projectile bullet = projectile.GetComponent<ME_Projectile>();

        bullet.damage = damage;
        bullet.damageFrequency = damageFrequency;
        bullet.speed = speed;
        bullet.rotationSpeed = rotationSpeed;
        bullet.lifespan = lifespan;
        bullet.transform.localScale = scale;

        return projectile;
    }

    public virtual GameObject BuildProjectileFragile(GameObject projectilePrefab)
    {
        GameObject projectile = InstantiateProjectile(projectilePrefab);

        return projectile;
    }

    public virtual GameObject BuildProjectileSturdy(GameObject projectilePrefab)
    {
        GameObject projectile = InstantiateProjectile(projectilePrefab);
        projectile.transform.SetParent(transform);

        return projectile;
    }
}
