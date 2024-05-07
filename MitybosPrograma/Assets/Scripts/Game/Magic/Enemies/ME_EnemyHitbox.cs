using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ME_EnemyHitbox : MonoBehaviour
{
    [SerializeField] private ME_Enemy enemy;
    private float activeCooldown = 0;

    // Update is called once per frame
    void Update()
    {
        if (activeCooldown > 0)
            activeCooldown -= Time.deltaTime;
    }

    public void DealDamage()
    {
        if (activeCooldown <= 0)
        {
            activeCooldown = enemy.DamageCooldown;
            enemy.Player.TakeDamage(enemy.Damage);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player Hurtbox")
        {
            DealDamage();
        }
    }
}
