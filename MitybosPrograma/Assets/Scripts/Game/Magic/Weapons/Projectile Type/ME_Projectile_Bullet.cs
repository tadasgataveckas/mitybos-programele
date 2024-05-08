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
            ME_Enemy enemy = collision.gameObject.GetComponent<ME_Enemy>();
            if (enemy != null)
            {
                DealDamage(enemy);
            }
            TriggerDestruction();
        }
    }
}
