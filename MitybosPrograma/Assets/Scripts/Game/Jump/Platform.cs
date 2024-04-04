using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 5f;

    private JumpGameManager jumpGameManager;
    private float screenHeightInUnits;

    // for not sending duplicate triggers
    private bool hasTriggeredAnimation = false;

    //Special Platform types:
    public bool BreakAfterTouch = false;


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
            if (!hasTriggeredAnimation)
            {
                collision.gameObject.GetComponent<JumpPlayerController>().Land();
                hasTriggeredAnimation = true;
                StartCoroutine(ResetAnimationTriger(0.3f));
            }

            if(BreakAfterTouch)
            {
                Destroy(gameObject);
            }
            
        }
    }
    // After delay
    private IEnumerator ResetAnimationTriger(float delay)
    {
        yield return new WaitForSeconds(delay);
        hasTriggeredAnimation = false; // Reset hasTriggeredAnimation after delay
    }



    // When to destroy

    void Start()
    {
        jumpGameManager = JumpGameManager.Instance;
        screenHeightInUnits = jumpGameManager.screenHeightInUnits;
    }

    void Update()
    {
        if (jumpGameManager != null && jumpGameManager.player.position.y - screenHeightInUnits > transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
