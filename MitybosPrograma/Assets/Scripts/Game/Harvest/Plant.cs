using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public Animator animator;
    public bool startGrowing;
    public float growthInterval = 2f; // Interval between growth stages

    public void StartGrowing(RuntimeAnimatorController plantAnimator)
    {
        // Assign the new animator controller
        animator.runtimeAnimatorController = plantAnimator;

        // Start the growth coroutine
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        while (true)
        {
            // Increase the "stage" variable of the animator by 1
            animator.SetFloat("stage", animator.GetFloat("stage") + 1);
            if (animator.GetFloat("stage") >= 6)
            {
                GetComponent<PickableObject>().disabled = false;
                yield return null;
            }
            yield return new WaitForSeconds(growthInterval);
        }
    }
}
