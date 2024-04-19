using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// When colliding with this objects BoxCollider,
// ExplodeOnEnter sets explode animation

public class ExplodeOnEnter : MonoBehaviour
{
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
            animator.SetTrigger("explode");
        }
    }

}
