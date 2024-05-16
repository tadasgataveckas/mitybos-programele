using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

public class JumpGameManager : MonoBehaviour
{
    // Singleton instance
    public static JumpGameManager Instance;

    public GameObject platformPrefab;
    public GameObject oneTimePlatformPrefab;
    public GameObject highJumpPlatformPrefab;
    public GameObject DeathPlatformPrefab;
    public GameObject relaxPlatformPrefab;
    public GameObject movePlatformPrefab;
    public GameObject coinPrefab;
    public GameObject shieldPrefab;

    public TextMeshProUGUI score;
    [HideInInspector] public float maxScore = 0f;
    public GameObject gameOver;

    public Transform player;
    public GameObject shieldSprite;
    [HideInInspector] public bool hasShield = false;

    private Vector3 spawnPosition = new Vector3();
    public Transform cameraTransform;

    private int SectionCounter = 0;
    [SerializeField] private int spawnRelaxStationsPerXft = 100;
    [SerializeField] private Scoreboard scoreboard;
    [HideInInspector] public int coins;
    [HideInInspector] public float screenWidthInUnits;
    [HideInInspector] public float screenHeightInUnits;

    ClientMethods c = new ClientMethods(new DatabaseMethods());
    private int id_user;
    private int intScore;
    private LevelCoins levelCoins;



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
        id_user = SessionManager.GetIdKey();

        coins = 0;
        SpawnSection();
    }

    // spawns section of platforms (relax platform included)
    private void SpawnSection()
    {
        SectionCounter++;
        spawnPosition.y += 2f;

        // spawn platforms until relax station bounds
        while (spawnPosition.y < spawnRelaxStationsPerXft * SectionCounter)
            GenerateRandomPlatform();

        // Relax Station
        spawnPosition.y += UnityEngine.Random.Range(.2f, 1f);
        Instantiate(relaxPlatformPrefab, new Vector3(0f, spawnPosition.y, 0f), Quaternion.identity);
        spawnPosition.y += 2f;

        Debug.Log("SECTION: " + SectionCounter);
    }

    private void GenerateRandomPlatform()
    {
        float percentage = UnityEngine.Random.Range(0f, 100f);

        spawnPosition.y += UnityEngine.Random.Range(.2f, 1f);
        spawnPosition.x = UnityEngine.Random.Range(0.2f - screenWidthInUnits / 2, screenWidthInUnits / 2 - 0.2f);
        switch (percentage)
        {
            // One time platform, 30%
            case < 30:
                SpawnPlatform(oneTimePlatformPrefab);
                break;
            // High jump platform, 10%
            case < 40:
                SpawnPlatform(highJumpPlatformPrefab);
                break;
            // High jump platform, 10%
            case < 60:
                SpawnPlatform(DeathPlatformPrefab);
                spawnPosition.x = UnityEngine.Random.Range(0.2f - screenWidthInUnits / 2, screenWidthInUnits / 2 - 0.2f);
                SpawnPlatform(platformPrefab);
                break;
            // Moving platform, 10%
            case < 70:
                SpawnPlatform(movePlatformPrefab);
                break;
            // Regular, 30%
            default:
                SpawnPlatform(platformPrefab);
                break;
        }
    }

    private void SpawnPlatform(GameObject platform)
    {
        Instantiate(platform, spawnPosition, Quaternion.identity);

        // added margin for pickup, fuck knows if this creates any impossible jumps
        spawnPosition.y += 0.2f;


        float percentage = UnityEngine.Random.Range(0f, 100f);

        // 10% coin spawn chance
        if (percentage < 10)
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        // 5% coin spawn chance
        else if (percentage < 15)
            Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
    }

    void Update()
    {
        // Update max score
        if (player.position.y > maxScore)
        {
            maxScore = player.position.y;
            score.text = maxScore.ToString("F1") + " ft";
        }
        intScore = (int)Math.Round(maxScore);

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

        // spawns section after player reaches relax platform
        if (player.position.y > spawnRelaxStationsPerXft * SectionCounter - 10)
        {
            SpawnSection();
        }
    }

    public void DamagePlayer()
    {
        if (hasShield)
        {
            shieldSprite.SetActive(false);
            hasShield = false;
        }
        else
            TriggerGameOver();
    }

    private void CountCameraSize()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        // Get the orthographic size of the camera
        float orthographicSize = mainCamera.orthographicSize;

        // Calculate the aspect ratio (width / height)
        float aspectRatio = Screen.width / (float)Screen.height;

        // Calculate the screen width in world units
        screenWidthInUnits = orthographicSize;// * 2 * aspectRatio;
        screenHeightInUnits = orthographicSize * 2;
    }

    public void RestartScene()
    {
        // Restart the scene
        Destroy(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    private void TriggerGameOver()
    {
        // Game over
        gameOver.SetActive(true);
        player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        PopulateScoreboard();

        // TODO: store coins somewhere
        Debug.Log("Collected coins: " + coins);
        //c.InsertUserLevelCoins(id_user, 0, 0, 0, 0);
        c.UpdateUserCoins(id_user, coins);
        c.UpdateUserXp(id_user, intScore);
        
        //
        //
        //

        // added so Update() won't run while game is over
        this.enabled = false;
    }

    //private int ReturnCoins()
    //{
    //    Debug.Log("Returned Coins: " + coins);
    //    
    //    return coins;
    //}

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
