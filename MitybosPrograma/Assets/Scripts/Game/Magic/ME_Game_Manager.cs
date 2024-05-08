using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ME_Game_Manager : MonoBehaviour
{
    public float Score = 0;

    public GameObject Player;
    public List<GameObject> Enemies;
    public float EnemySpawnDelay = 0.1f;

    private float lastSpawnTime = 1;
    private int enemyTotalCounter = 0;

    [SerializeField] private GameObject gameOver;
    [SerializeField] private Scoreboard scoreboard;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(EnemySpawner());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator EnemySpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(EnemySpawnDelay);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (Enemies.Count > 0)
        {
            enemyTotalCounter++;

            int enemyIndex = Random.Range(0, Enemies.Count - 1);
            GameObject enemy = Instantiate(Enemies[0], GenerateSpawnLocation(), Quaternion.identity);
            //enemy.transform.SetParent(Player.transform.parent, false);
            //enemy.GetComponent<ME_Enemy>().Target = Player;
        }
    }

    private Vector2 GenerateSpawnLocation()
    {
        float randX = Player.transform.position.x;
        float randY = Player.transform.position.y;
        float margin = 6f;

        switch (Random.Range(0, 4))
        {
            case 0:     //left
                randX += -margin;
                randY += Random.Range(-margin, margin);
                break;
            case 1:     //right
                randX += margin;
                randY += Random.Range(-margin, margin);
                break;
            case 2:     //top
                randX += Random.Range(-margin, margin);
                randY += margin;
                break;
            default:    //bottom
                randX += Random.Range(-margin, margin);
                randY += -margin;
                break;
        }
        return new Vector2(randX, randY);
    }

    public void TriggerGameEnd()
    {
        // important, so game logic doesn't run continuously
        Time.timeScale = 0;

        // Game over
        gameOver.SetActive(true);
        PopulateScoreboard();

        this.enabled = false;
    }

    // inserts score into db and populates scoreboard
    private void PopulateScoreboard()
    {
        int id_user = SessionManager.GetIdKey();
        if (id_user < 0)
            return;

        long id_score = ScoresMagicExpedition.SaveScore(id_user, Score);
        List<ScoresMagicExpedition> list = ScoresMagicExpedition.GetScoresById(id_user);

        // finds and highlights current score index
        int currentScoreIndex = -1;
        for (int i = 0; i < list.Count; i++)
            if (list[i].id_score == id_score)
            {
                // if within top 10
                if (i < 10)
                    scoreboard.HighlightElement(i);
                // if lower position than 10
                else
                    scoreboard.HighlightElement(9);

                currentScoreIndex = i;
                break;
            }


        // add 10 scores, if current is not in the top 10, show in place of 10th
        for (int i = 0; i < 10; i++)
        {
            // in case list is shorter than 10
            if (i >= list.Count)
                break;

            // if score doesn't reach top 10
            if (i == 9 && currentScoreIndex > 9)
            {
                scoreboard.AddScoreItem((currentScoreIndex + 1).ToString());
                scoreboard.AddScoreItem(System.Math.Round(list[currentScoreIndex].score, 1).ToString() + " pts").alignment = TextAlignmentOptions.Left;
                scoreboard.AddScoreItem(list[i].score_date.Substring(0, list[currentScoreIndex].score_date.Length - 9));
            }
            else
            {
                scoreboard.AddScoreItem((i + 1).ToString());
                scoreboard.AddScoreItem(System.Math.Round(list[i].score, 1).ToString() + " pts").alignment = TextAlignmentOptions.Left;
                scoreboard.AddScoreItem(list[i].score_date.Substring(0, list[i].score_date.Length - 9));
            }
        }
    }

    public void RestartScene()
    {
        // Restart the scene
        Destroy(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
