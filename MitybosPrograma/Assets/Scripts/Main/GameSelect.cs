using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSelect : MonoBehaviour
{
    public string gameSceneName;

    public void ChangeToGameScene()
    {
        if (gameSceneName != "")
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}
