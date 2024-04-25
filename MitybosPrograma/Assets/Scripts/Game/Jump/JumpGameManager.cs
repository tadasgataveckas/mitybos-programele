using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

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

    public int spawnRelaxStationsPerXft = 100;
    [SerializeField] private Scoreboard scoreboard;

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
            spawnPosition.y += UnityEngine.Random.Range(.2f, 1f);
            spawnPosition.x = UnityEngine.Random.Range(0.2f - screenWidthInUnits / 2, screenWidthInUnits / 2 - 0.2f);
            // Relax Station
            if ((int)spawnPosition.y % spawnRelaxStationsPerXft == 0 && (int)spawnPosition.y != 0)
            {
                Instantiate(relaxPlatformPrefab, new Vector3(0f, spawnPosition.y, 0f), Quaternion.identity);
                spawnPosition.y += 2f;
            }
            else // Regular
            {
                float percentage = UnityEngine.Random.Range(0f, 100f);
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
        // Update max score
        if (player.position.y > maxScore)
        {
            maxScore = player.position.y;
            score.text = maxScore.ToString("F1") + " ft";
        }

        // Check if player is below the camera vertically
        if (player.position.y < cameraTransform.position.y - screenHeightInUnits / 2)
        {
            TriggerGameOver();
        }

        // Check if player moves outside camera horizontally
        if (Mathf.Abs(player.position.x - cameraTransform.position.x) - 0.1f > screenWidthInUnits / 2)
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

    public void RestartScene()
    {
        // Restart the scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void TriggerGameOver()
    {
        // Game over
        gameOver.SetActive(true);
        player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        PopulateScoreboard();

        // added so Update() won't run while game is over
        this.enabled = false;
    }

    // inserts score into db and populates scoreboard
    private void PopulateScoreboard()
    {
        int id_user = SessionManager.GetIdKey();
        if (id_user < 0)
            return;

        ScoresChickenWings.SaveScore(id_user, maxScore);
        List<ScoresChickenWings> list = ScoresChickenWings.GetScoresById(id_user);

        // finds and highlights current score index
        int currentScoreIndex = -1;
        for (int i = 0; i < list.Count; i++)
        {
            if (System.Math.Round(list[i].score) == System.Math.Round(maxScore))
            {
                // if within top 10
                if (i < 10)
                {
                    scoreboard.HighlightElement(i);
                    break;
                }
                // if lower position than 10
                else
                {
                    scoreboard.HighlightElement(9);
                    currentScoreIndex = i;
                    break;
                }
            }
        }

        // add 10 scores, if current is not in the top 10, show in place of 10th
        for (int i = 0; i <= 9; i++)
        {
            // in case list is shorter than 10
            if (i >= list.Count)
                break;

            // if score doesn't reach top 10
            if (i == 9 && currentScoreIndex > 9)
            {
                scoreboard.AddScoreItem((currentScoreIndex + 1).ToString());
                scoreboard.AddScoreItem(Math.Round(list[currentScoreIndex].score, 1).ToString() + " ft").alignment = TextAlignmentOptions.Left;
                scoreboard.AddScoreItem(list[i].score_date.Substring(0, list[currentScoreIndex].score_date.Length - 9));
            }
            else
            {
                scoreboard.AddScoreItem((i + 1).ToString());
                scoreboard.AddScoreItem(Math.Round(list[i].score, 1).ToString() + " ft").alignment = TextAlignmentOptions.Left;
                scoreboard.AddScoreItem(list[i].score_date.Substring(0, list[i].score_date.Length - 9));
            }
        }
    }
}
