using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ME_Entity : MonoBehaviour
{
    public float HP;
    public Healthbar healthbar;
    public float MaxHP;

    public void TakeDamage(float damage)
    {
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
