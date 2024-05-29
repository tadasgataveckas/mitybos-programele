using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : MonoBehaviour
{
    [SerializeField] // Ensure this field is visible in the Unity Editor
    public List<PlateFood> possibleFoods = new List<PlateFood>(); 
    int chosenFoodIndex = 0;
    public Transform plateFood;
    public float plateFoodFlySpeed = 5f; // Adjust as needed
    bool flyFood;
    public float heightFromPlayer = 1.5f;

    private Animator animator;
    private JumpGameManager jumpGameManager;
    private Transform playerTransform;

    private Transform VitaminTooltipHolder;
    private GameObject spawnedTooltip;

    void Start()
    {
        chosenFoodIndex = Random.Range(0, possibleFoods.Count);
        plateFood.GetComponent<Animator>().runtimeAnimatorController = possibleFoods[chosenFoodIndex].animator;
        jumpGameManager = JumpGameManager.Instance;
        playerTransform = jumpGameManager.player;
        VitaminTooltipHolder = jumpGameManager.VitaminTooltipHolder;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (flyFood && playerTransform != null)
        {
            // Calculate the target position with the same x coordinate as the player
            Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y+ heightFromPlayer, plateFood.position.z);

            // Move towards the target position using MoveTowards for linear movement
            plateFood.position = Vector3.MoveTowards(plateFood.position, targetPosition, plateFoodFlySpeed * Time.deltaTime);

            float tolerance = 0.01f;
            bool foodFlownToTargetPosition = Vector3.Distance(plateFood.position, targetPosition) < tolerance;

            if (foodFlownToTargetPosition)
            {
                flyFood = false;
                plateFood.GetComponent<PlateFoodObject>().vitaminName = possibleFoods[chosenFoodIndex].vitaminName;
                plateFood.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    //Chair went to its final position
    public void SpawnTooltip()
    {
        if (spawnedTooltip == null)
        {
            spawnedTooltip = Instantiate(possibleFoods[chosenFoodIndex].tooltipPrefab, VitaminTooltipHolder.position, Quaternion.identity, VitaminTooltipHolder);
        }
    }
    public void PlayerTriesToJump() {
        StartCoroutine(StartFoodFly(0.05f));
    }
    IEnumerator StartFoodFly(float delay)
    {
        animator.SetTrigger("happy");

        yield return new WaitForSeconds(delay);
        flyFood = true;
        Destroy(spawnedTooltip);
    }
}

[System.Serializable] // This attribute ensures that the class can be serialized by Unity
public class PlateFood
{
    public RuntimeAnimatorController animator;
    public string vitaminName;
    public GameObject tooltipPrefab;
}
