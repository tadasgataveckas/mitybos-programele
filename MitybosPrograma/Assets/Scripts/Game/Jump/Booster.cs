using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// When colliding with this objects BoxCollider,
// Booster script changes current velocity with setVelocityTo,
// ensuring Player does not overjump the Relax Station (completely missing it and its rewards)

public class Booster : MonoBehaviour
{
    public float setVelocityTo = 5f;
    public GameObject chair;
    public GameObject chef;


    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 velocity = rb.velocity;
            velocity.y = setVelocityTo;
            rb.velocity = velocity;
            chair.GetComponent<ChairSlide>().slidingTarget = "Player";
            chef.SetActive(true);
            other.GetComponent<JumpPlayerController>().currentChef = chef;
        }
    }

}
