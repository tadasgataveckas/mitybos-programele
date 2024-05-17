using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Tracker_Projectile : ME_Projectile
{
    public ME_Projectile projectilePrefab;
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
            TriggerDestruction();
        }
    }

    public override void OnDestructionEvent()
    {
        base.OnDestructionEvent();
        ME_Projectile projectile =  InstantiateProjectile(projectilePrefab);
        projectile.transform.rotation = Quaternion.identity;

    }
}
