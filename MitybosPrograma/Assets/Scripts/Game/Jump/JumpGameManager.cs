using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGameManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public int platformCount = 300;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPosition = new Vector3();

        for (int i = 0; i < platformCount; i++)
        {
            spawnPosition.y += Random.Range(.2f, 1f);
            spawnPosition.x = Random.Range(-1f, 1f);
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
