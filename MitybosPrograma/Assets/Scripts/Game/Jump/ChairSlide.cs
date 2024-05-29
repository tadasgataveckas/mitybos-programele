using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairSlide : MonoBehaviour
{
    private JumpGameManager jumpGameManager;
    public string slidingTarget = "";
    public Transform finalSpotTransform;
    private Transform playerTransform;
    public float slideSpeed = 5f; // Adjust as needed
    public Animator chef;

    void Start()
    {
        jumpGameManager = JumpGameManager.Instance;
        playerTransform = jumpGameManager.player;
    }

    void Update()
    {
        if (jumpGameManager != null && jumpGameManager.player.position.y - jumpGameManager.screenHeightInUnits > transform.position.y)
        {
            Destroy(gameObject);
        }

        if (slidingTarget == "Player" && playerTransform != null)
        {
            // Calculate the target position with the same x coordinate as the player
            Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);

            // Move towards the target position using MoveTowards for linear movement
            transform.position = Vector3.MoveTowards(transform.position, targetPosition-new Vector3(0.01f, 0f, 0f), slideSpeed * Time.deltaTime);
        }

        if (slidingTarget == "FinalSpot" && playerTransform != null)
        {
            Vector3 targetPositionChair = new Vector3(finalSpotTransform.position.x, transform.position.y, transform.position.z);
            Vector3 targetPositionPlayer = new Vector3(finalSpotTransform.position.x, playerTransform.position.y, playerTransform.position.z);

            // Move both the player and this object towards the final spot
            transform.position = Vector3.MoveTowards(transform.position, targetPositionChair - new Vector3(0.01f, 0f, 0f), slideSpeed * Time.deltaTime);
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPositionPlayer, slideSpeed * Time.deltaTime);

            // Check if both objects have reached their target positions
            float tolerance = 0.01f; // Adjust as needed
            bool chairReached = Vector3.Distance(transform.position, targetPositionChair) < tolerance;

            if (chairReached)
            {
                slidingTarget = "";
                chef.gameObject.GetComponent<Chef>().SpawnTooltip();
                //StartCoroutine(SetHappyTriggerAfterDelay(0.5f)); //
            }
        }
    }
    IEnumerator SetHappyTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        chef.SetTrigger("happy");
    }
}
