using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Game_Manager : MonoBehaviour
{
    public GameObject Player;
    public List<GameObject> Enemies;
    public float EnemySpawnDelay = 0.1f;
    private float lastSpawnTime = 1;
    [HideInInspector] int enemyTotalCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawnTime >= EnemySpawnDelay)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }

    }

    private void SpawnEnemy()
    {
        if (Enemies.Count > 0)
        {
            enemyTotalCounter++;

            int enemyIndex = Random.Range(0, Enemies.Count - 1);
            GameObject enemy = Instantiate(Enemies[0], new Vector2(0, 0), Player.transform.rotation);
            enemy.transform.SetParent(Player.transform.parent, false);
            enemy.GetComponent<ME_Enemy>().Target = Player;
        }
    }
}
