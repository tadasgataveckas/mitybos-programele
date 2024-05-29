using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public float jumpForce = 0f;

    private JumpGameManager jumpGameManager;
    private float screenHeightInUnits;
    public Chef chef;

    // for not sending duplicate triggers
    private bool hasTriggeredAnimation = false;


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
            chef.SpawnTooltip();
            if (!hasTriggeredAnimation)
            {
                collision.gameObject.GetComponent<JumpPlayerController>().LandSit();
                collision.gameObject.GetComponent<JumpPlayerController>().sitting = true;
                GetComponent<ChairSlide>().slidingTarget = "FinalSpot";
                hasTriggeredAnimation = true;
                StartCoroutine(ResetAnimationTriger(0.3f));
                //Spawn food?
                //Go to designated position
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
