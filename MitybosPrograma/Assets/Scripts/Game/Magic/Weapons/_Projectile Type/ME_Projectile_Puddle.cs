using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Projectile_Puddle : ME_Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        TriggerTimedDestruction();
        StartCoroutine(DealContinuousAOEDamage());
        StartCoroutine(ShrinkAnimation());
    }

    private IEnumerator ShrinkAnimation()
    {
        Vector3 originalScale = transform.localScale;
        float timer = 0f;

        while (timer < lifespan)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.one, timer / lifespan);
            yield return null;
        }
    }
}