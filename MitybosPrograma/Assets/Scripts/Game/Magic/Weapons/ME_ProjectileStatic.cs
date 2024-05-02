using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_ProjectileStatic : MonoBehaviour
{
    [HideInInspector] public float damage = 0f;
    // degrees per second
    [HideInInspector] public float speed = 180f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ME_Enemy enemy = collision.gameObject.GetComponent<ME_Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }
    }
}
