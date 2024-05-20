using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Projectile_Sword : ME_Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DealContinuousAOEDamage());
    }

    // Update is called once per frame
    void Update()
    {
        RotateProjectile();
    }
}
