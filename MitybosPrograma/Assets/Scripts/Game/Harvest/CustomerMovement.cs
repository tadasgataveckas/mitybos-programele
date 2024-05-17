using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Vector3 targetPosition;
    private string destinationName;
    public bool isMoving = false;
    public Animator animator;
    public SpriteRenderer spriteRenderer;


    void Update()
    {
        if (isMoving)
        {
            MoveToTarget();
            SetWalkAnimation(true);
        }
        else
        {
            SetWalkAnimation(false);
        }
    }

    public void SetTargetPosition(Vector3 position, string destination)
    {
        targetPosition = position;
        isMoving = true;
        destinationName = destination;
    }

    private void MoveToTarget()
    {
        Vector3 direction = targetPosition - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        FlipSprite(direction.x);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            OnReachedTarget();
        }
    }

    private void SetWalkAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("walk", isWalking);
        }
    }

    private void FlipSprite(float directionX)
    {
        if (spriteRenderer != null)
        {
            if (directionX > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (directionX < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void OnReachedTarget()
    {
        // Implement behavior once the customer reaches the target
    }
    public void SitAtTable(Vector3 tablePosition)
    {
        // Immediately flip the sprite
        spriteRenderer.flipX = transform.position.x > tablePosition.x;

        // Start the coroutine to delay the sit animation
        StartCoroutine(DelayedSitAnimation());
    }

    private IEnumerator DelayedSitAnimation()
    {
        // Wait for 1 second before starting the sit animation
        yield return new WaitForSeconds(1f);

        // Start the sit animation
        if (animator != null)
        {
            animator.SetBool("sit", true);
        }
    }

    public void StandUp()
    {
        animator.SetBool("sit", false);
    }

}
