using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 5f;

    private JumpGameManager jumpGameManager;
    private float screenHeightInUnits;

    // for not sending duplicate triggers
    private bool hasTriggeredAnimation = false;

    //Special Platform types:
    public bool breakAfterTouch = false;
    public bool killOnTouch = false;

    // all for movement
    public bool moveAround = false;
    private float speed = 1f;
    private bool moveLeft = true;


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

            if (killOnTouch)
            {
                jumpGameManager.DamagePlayer();
            }

            if (breakAfterTouch)
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
        if (moveAround)
        {
            MoveInDirection();
        }
    }

    private void MoveInDirection()
    {
        Vector3 target = transform.position;
        if (moveLeft)
        {
            target.x = 0.2f - jumpGameManager.screenWidthInUnits / 2;
            if (transform.position.x <= target.x)
                moveLeft = false;
        }
        else
        {
            target.x = jumpGameManager.screenWidthInUnits / 2 - 0.2f;
            if (transform.position.x >= target.x)
                moveLeft = true;
        }
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
