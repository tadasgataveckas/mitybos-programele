using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateFoodObject : MonoBehaviour
{
    public string vitaminName;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            JumpGameManager.Instance.ConsumeVitamin(vitaminName);
            animator.SetTrigger("explode");
        }
    }

}
