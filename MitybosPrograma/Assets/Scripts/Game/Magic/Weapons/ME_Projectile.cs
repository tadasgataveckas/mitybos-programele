using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ME_Projectile : MonoBehaviour
{
    [HideInInspector] public float damage = 0f;
    [HideInInspector] public float damageCooldown = 0.2f;
    [HideInInspector] public float speed = 10f;
    [HideInInspector] public float rotationSpeed = 180f;
    [HideInInspector] public float lifespan = 3f;

    public void TriggerTimedDestruction()
    {
        Destroy(gameObject, lifespan);
    }

    public void TriggerDestruction()
    {
        Destroy(gameObject);
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
        return gameObject.gameObject.CompareTag("Enemy");
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
        Debug.Log("Shooting aoe!");
        // collision list
        List<Collider2D> colliders = new List<Collider2D>();

        // filters through enemy collision
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Enemy"));

        Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, colliders);

        foreach (Collider2D collider in colliders)
        {
            ME_Enemy enemy = collider.gameObject.GetComponent<ME_Enemy>();
            if (enemy != null)
                DealDamage(enemy);
        }
    }
}
