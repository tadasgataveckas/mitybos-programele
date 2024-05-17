using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class ME_Projectile : MonoBehaviour
{
    [HideInInspector] public float damage = 0f;
    [HideInInspector] public float damageCooldown = 0.2f;
    [HideInInspector] public float speed = 10f;
    [HideInInspector] public float rotationSpeed = 180f;
    [HideInInspector] public float lifespan = 3f;

    public IEnumerator TriggerTimedDestruction()
    {
        yield return new WaitForSeconds(lifespan);
        TriggerDestruction();
    }

    public void TriggerDestruction()
    {
        StopAllCoroutines();
        OnDestructionEvent();
        Destroy(gameObject);
    }

    public virtual void OnDestructionEvent()
    {
        // to overwrite
    }

    // starts straight line movement
    public void TriggerMovement()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = rb.transform.right;
        rb.velocity = direction * speed;
    }

    // base damage function
    public void DealDamage(ME_Enemy enemy)
    {
        if (enemy != null)
            enemy.TakeDamage(damage);
    }

    // checks if object is enemy
    public bool IsEnemy(GameObject gameObject)
    {
        return gameObject.CompareTag("Enemy Hurtbox");
    }

    // checks if object is enemy
    public ME_Enemy GetEnemyFromCollision(Collider2D collider)
    {
        return collider.GetComponent<ME_EnemyHurtbox>().enemy;
    }

    // rotation should be called every frame
    public void RotateProjectile()
    {
        if (rotationSpeed != 0)
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    // Coroutine to deal damage over time
    public IEnumerator DealContinuousAOEDamage()
    {
        while (true)
        {
            DealAOEDamage();
            yield return new WaitForSeconds(damageCooldown);
        }
    }

    // damage to enemies within the trigger area
    public void DealAOEDamage()
    {
        // collision list
        List<Collider2D> colliders = new List<Collider2D>();

        // filters through and picks up anything in "Enemy" layer (not tag)
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Enemy Hurtbox"));

        Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, colliders);

        foreach (Collider2D collider in colliders)
        {
            if (IsEnemy(collider.gameObject))
            {
                ME_Enemy enemy = GetEnemyFromCollision(collider);
                if (enemy != null)
                    DealDamage(enemy);
            }
        }
    }

    public IEnumerator ShrinkAnimation()
    {
        Vector3 originalScale = transform.localScale;
        float timer = 0f;

        while (timer < lifespan)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.one, timer / lifespan);
            yield return null;
        }
    }

    public IEnumerator EnlargeAnimation()
    {
        Vector3 originalScale = transform.localScale;
        float timer = 0f;

        while (timer < lifespan)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, timer / lifespan);
            yield return null;
        }
    }

    public ME_Projectile InstantiateProjectile(ME_Projectile projectilePrefab)
    {
        ME_Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        projectile.damage = damage;
        projectile.damageCooldown = damageCooldown;
        projectile.speed = speed;
        projectile.rotationSpeed = rotationSpeed;
        projectile.lifespan = lifespan;
        projectile.transform.localScale = Vector3.Scale(projectile.transform.localScale, transform.localScale * 2);

        return projectile;
    }
}
