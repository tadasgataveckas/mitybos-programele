using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ME_Entity : MonoBehaviour
{
    public float HP;
    public Healthbar healthbar;
    public float MaxHP;
    public bool isDead = false;

    public virtual void TakeDamage(float damage)
    {
        if (this == null || isDead)
            return;

        HP -= damage;
        UpdateHealthBar();

        if (HP <= 0)
            Die();
    }

    public virtual void UpdateHealthBar()
    {

    }

    public virtual void Die()
    {

    }
}
