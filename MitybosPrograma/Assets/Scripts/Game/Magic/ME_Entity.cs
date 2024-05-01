using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ME_Entity : MonoBehaviour
{
    [HideInInspector] public float HP;
    public float MaxHP;

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage taken");

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
