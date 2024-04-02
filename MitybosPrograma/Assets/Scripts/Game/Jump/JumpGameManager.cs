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
    public int platformCount = 300;
    //
    public TextMeshProUGUI score;
    [HideInInspector] public float maxScore = 0f;
    public GameObject gameOver;

    public Transform player;
    public Transform cameraTransform;

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
    }

    void Start()
    {
        CountCameraSize();
        Vector3 spawnPosition = new Vector3();

        for (int i = 0; i < platformCount; i++)
        {
            spawnPosition.y += Random.Range(.2f, 1f);
            spawnPosition.x = Random.Range(-1f, 1f);
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void Update()
    {
        if (player.position.y > maxScore)
        {
            maxScore = player.position.y;
            score.text = maxScore.ToString("F1") + " ft";
        }
        if (player.position.y < cameraTransform.position.y - screenHeightInUnits/2)
        {
            gameOver.SetActive(true);
            player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
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
