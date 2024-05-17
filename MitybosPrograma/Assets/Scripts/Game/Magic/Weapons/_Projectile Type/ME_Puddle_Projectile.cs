using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Puddle_Projectile : ME_Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TriggerTimedDestruction());
        StartCoroutine(DealContinuousAOEDamage());
        StartCoroutine(ShrinkAnimation());
    }
}