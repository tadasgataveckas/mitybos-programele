using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Explosion_Projectile : ME_Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        lifespan = 0.5f;
        StartCoroutine(EnlargeAnimation());
        DealAOEDamage();
        StartCoroutine(TriggerTimedDestruction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
