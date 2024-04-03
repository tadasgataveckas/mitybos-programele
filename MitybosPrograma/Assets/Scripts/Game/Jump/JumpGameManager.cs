using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class JumpGameManager : MonoBehaviour
{
    public static JumpGameManager Instance; // Singleton instance

    public GameObject platformPrefab;
    public GameObject oneTimePlatformPrefab;
    public GameObject highJumpPlatformPrefab;
    public GameObject relaxPlatformPrefab;
    public int platformCount = 300;
    //
    public TextMeshProUGUI score;
    [HideInInspector] public float maxScore = 0f;
    public GameObject gameOver;

    public Transform player;
    public Transform cameraTransform;

    private int spawnRelaxStationsPerXft = 100;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of JumpGameManager detected!");
            Destroy(gameObject);
        }
        CountCameraSize();
    }

    void Start()
    {
        Vector3 spawnPosition = new Vector3();

        for (int i = 0; i < platformCount; i++)
        {
            spawnPosition.y += Random.Range(.2f, 1f);
            spawnPosition.x = Random.Range(0.2f-screenWidthInUnits/2, screenWidthInUnits / 2 - 0.2f);
            // Relax Station
            if ((int)spawnPosition.y % spawnRelaxStationsPerXft == 0 && (int)spawnPosition.y != 0)
            {
                Instantiate(relaxPlatformPrefab, new Vector3(0f, spawnPosition.y, 0f), Quaternion.identity);
                spawnPosition.y += 2f;
            } 
            else // Regular
            {
                float percentage = Random.Range(0f, 100f);
                // One time platform, 30%
                if (percentage < 30)
                {
                    Instantiate(oneTimePlatformPrefab, spawnPosition, Quaternion.identity);
                }
                // High jump platform, 10%
                else if (percentage < 40) 
                {
                    Instantiate(highJumpPlatformPrefab, spawnPosition, Quaternion.identity);
                }
                // Regular, 60%
                else
                {
                    Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
                }
                
            }
        }
    }

    void Update()
    {
        if (player.position.y > maxScore)
        {
            maxScore = player.position.y;
            score.text = maxScore.ToString("F1") + " ft";
        }
        // Check if player is below the camera vertically
        if (player.position.y < cameraTransform.position.y - screenHeightInUnits / 2)
        {
            // Game over
            gameOver.SetActive(true);
            player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        // Check if player moves outside camera horizontally
        if (Mathf.Abs(player.position.x - cameraTransform.position.x)-0.1f > screenWidthInUnits / 2)
        {
            // Teleport the player to the other side of the camera horizontally
            Vector3 playerPos = player.position;
            playerPos.x = cameraTransform.position.x - Mathf.Sign(playerPos.x - cameraTransform.position.x) * screenWidthInUnits / 2;
            player.position = playerPos;
        }
    }

    [HideInInspector] public float screenWidthInUnits;
    [HideInInspector] public float screenHeightInUnits;

    private void CountCameraSize()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        // Get the orthographic size of the camera
        float orthographicSize = mainCamera.orthographicSize;

        // Calculate the aspect ratio (width / height)
        float aspectRatio = Screen.width / (float)Screen.height;

        // Calculate the screen width in world units
        screenWidthInUnits = orthographicSize * 2 * aspectRatio;

        screenHeightInUnits = orthographicSize * 2;

        // Log the screen height in units
        //Debug.Log("Screen height in units: " + screenHeightInUnits);

        // Log the screen width in units
        //Debug.Log("Screen width in units: " + screenWidthInUnits);
    }
}
