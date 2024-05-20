using UnityEngine;

public class ME_Bullet_Projectile : ME_Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TriggerTimedDestruction());
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
    }
}
