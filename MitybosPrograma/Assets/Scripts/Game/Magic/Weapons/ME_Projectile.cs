using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Projectile : MonoBehaviour
{
    [HideInInspector] public float lifespan = 3f;
    [HideInInspector] public float damage = 0f;
    [HideInInspector] public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);

        // movement forward (right)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = rb.transform.right;
        rb.velocity = direction * speed;
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("Environment"))
        //    TriggerDestruction();
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ME_Enemy enemy = collision.gameObject.GetComponent<ME_Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
            TriggerDestruction();
        }
    }

    private void TriggerDestruction()
    {
        Destroy(gameObject);
    }
}
