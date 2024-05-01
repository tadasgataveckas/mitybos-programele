using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Enemy : ME_Entity
{
    public float speed = 1f;
    public float damage = 5f;

    public GameObject Target;
    public Healthbar Healthbar;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
            MoveToTarget(Target.transform.position - transform.position);
    }

    private void MoveToTarget(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

    public override void UpdateHealthBar()
    {
        Healthbar.UpdateHealthbar();
    }
}
