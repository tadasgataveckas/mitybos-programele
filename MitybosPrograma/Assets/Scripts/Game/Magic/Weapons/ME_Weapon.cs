using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ME_Weapon : MonoBehaviour
{
    public string weaponName = "";
    public int projectileCount = 2;
    public float shotCooldown = 0.5f;

    private float lastShotTime;

    // transferred stats 
    public float damage = 1;
    public float damageCooldown = 0.2f;
    public float speed = 1f;
    public float rotationSpeed = 180f;
    public float lifespan = 2;
    public Vector3 scale = Vector3.one;

    public void Shoot()
    {
        if (Time.time - lastShotTime >= shotCooldown)
        {
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
        bullet.damageCooldown = damageCooldown;
        bullet.speed = speed;
        bullet.rotationSpeed = rotationSpeed;
        bullet.lifespan = lifespan;
        bullet.transform.localScale = scale;

        return projectile;
    }

    public virtual GameObject BuildProjectileOnWorld(GameObject projectilePrefab)
    {
        GameObject projectile = InstantiateProjectile(projectilePrefab);

        return projectile;
    }

    public virtual GameObject BuildProjectileOnPlayer(GameObject projectilePrefab)
    {
        GameObject projectile = InstantiateProjectile(projectilePrefab);
        projectile.transform.SetParent(transform);

        return projectile;
    }

    public IEnumerator StartPeriodicDisable(GameObject gameObject)
    {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        LineRenderer renderer = collider.GetComponent<LineRenderer>();

        while (shotCooldown > 0)
        {
            collider.enabled = false;
            renderer.enabled = false;
            yield return new WaitForSeconds(shotCooldown);
            collider.enabled = true;
            renderer.enabled = true;
            yield return new WaitForSeconds(lifespan);
        }
    }

    public IEnumerator StartProjectileRotation(GameObject gameObject)
    {
        while (rotationSpeed != 0)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // rotation should be called every frame
    public void RotateProjectile()
    {
        if (rotationSpeed != 0)
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public virtual void ResetWeapon()
    {

    }
}
