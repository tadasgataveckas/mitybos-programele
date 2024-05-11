using UnityEngine;

public class ME_Projectile_Bullet : ME_Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        TriggerTimedDestruction();
        TriggerMovement();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnemy(collision.gameObject))
        {
            ME_Enemy enemy = GetEnemyFromCollision(collision);
            if (enemy != null)
            {
                DealDamage(enemy);
            }
            TriggerDestruction();
        }
        else
        {
            Debug.Log("failed, name: " + collision.gameObject.name);
        }
    }
}
