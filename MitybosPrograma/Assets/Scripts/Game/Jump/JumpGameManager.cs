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

    private int coinDropChance = 5;
    private int nutDropChance = 1;

    public Transform VitaminTooltipHolder;



    public List<float[]> PlatformPercentages = new List<float[]>()
    {
        // OneTime | HighJump | Death | Moving | Regular

        new float[] { 10f, 20f, 0f, 0f, 70f }, // Level 1 percentages
        new float[] { 20f, 20f, 10f, 0f, 50f },  // Level 2 percentages
        new float[] { 20f, 20f, 15f, 20f, 25f },  // Level 3 percentages
        new float[] { 20f, 20f, 20f, 20f, 20f },  // Level 4 percentages
        new float[] { 30f, 10f, 20f, 30f, 10f }  // Level 5 percentages

        // Add more levels as needed
    };

    private float[] currentPlatformPercentages;

    private void setPlatformPercentages(int index)
    {
        if (index >= PlatformPercentages.Count)
        {
            index = PlatformPercentages.Count - 1;
        }
        currentPlatformPercentages = PlatformPercentages[index];
        //convert
        for (int i = 1; i < 5; i++)
        {
            currentPlatformPercentages[i] = currentPlatformPercentages[i] + currentPlatformPercentages[i - 1];
        }
        Debug.Log(currentPlatformPercentages[0].ToString() + " " + currentPlatformPercentages[3].ToString());

    }


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
        // retrieves level_coins into object
        levelCoins = new LevelCoins(id_user);
        levelCoins.SynchData();

        coins = 0;
        SpawnSection();
    }

    // spawns section of platforms (relax platform included)
    private void SpawnSection()
    {
        setPlatformPercentages(SectionCounter);
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

        float OneTimePercentage = currentPlatformPercentages[0];
        float HighJumpPercentage = currentPlatformPercentages[1];
        float DeathPercentage = currentPlatformPercentages[2];
        float MovingPercentage = currentPlatformPercentages[3];



        // One time platform, 30%
        if (percentage < OneTimePercentage)
        {
            SpawnPlatform(oneTimePlatformPrefab);
        }
        // High jump platform, 10%
        else if (percentage < HighJumpPercentage)
        {
            SpawnPlatform(highJumpPlatformPrefab);
        }
        // Death platform, 20%
        else if (percentage < DeathPercentage)
        {
            SpawnPlatform(DeathPlatformPrefab);
            spawnPosition.x = UnityEngine.Random.Range(0.2f - screenWidthInUnits / 2, screenWidthInUnits / 2 - 0.2f);
            SpawnPlatform(platformPrefab);
        }
        // Moving platform, 10%
        else if (percentage < MovingPercentage)
        {
            SpawnPlatform(movePlatformPrefab);
        }
        // Regular, 30%
        else
        {
            SpawnPlatform(platformPrefab);
        }
    }


    private void SpawnPlatform(GameObject platform)
    {
        Instantiate(platform, spawnPosition, Quaternion.identity);

        // added margin for pickup, fuck knows if this creates any impossible jumps
        spawnPosition.y += 0.2f;


        float percentage = UnityEngine.Random.Range(0f, 100f);

        // 10% coin spawn chance
        if (percentage < coinDropChance)
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        // 5% shield spawn chance
        else if (percentage < nutDropChance)
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

    public void ConsumeVitamin(string vitaminName)
    {
        Debug.Log("Consumed " + vitaminName);
        if (vitaminName == "E")
        {
            // give shield
            hasShield = true;
            shieldSprite.SetActive(true);
        }
        if (vitaminName == "A")
        {
            // increase coin chance
            coinDropChance = (coinDropChance + 3) % 15; // max 15 
        }
        if (vitaminName == "C")
        {
            // increase shield drop chance
            nutDropChance = (nutDropChance + 3) % 15; // max 15 
        }
        if (vitaminName == "D")
        {
            // increase jump force
            player.GetComponent<JumpPlayerController>().jumpForce += 0.2f; 
        }
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
        //c.UpdateUserLevel(id_user, levelCoins.xp / 10);

        //
        //
        //

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
