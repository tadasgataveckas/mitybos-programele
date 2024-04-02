using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 5f;

    private JumpGameManager jumpGameManager;
    private float screenHeightInUnits;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 velocity = rb.velocity;
                velocity.y = jumpForce;
                rb.velocity = velocity;
            }
            collision.gameObject.GetComponent<JumpPlayerController>().Land();
        }
    }

    // When to destroy

    void Start()
    {
        jumpGameManager = JumpGameManager.Instance;
        screenHeightInUnits = jumpGameManager.screenHeightInUnits;
    }

    void Update()
    {
        if (jumpGameManager != null && jumpGameManager.player.position.y - screenHeightInUnits/2 - 0.5f > transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
