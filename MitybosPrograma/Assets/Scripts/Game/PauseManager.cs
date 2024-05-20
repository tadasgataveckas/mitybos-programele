using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject pauseScreen;

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseButton.SetActive(false);
        pauseScreen.SetActive(true);
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void PauseUniversal()
    {
        if (Time.timeScale == 0)
            UnpauseGame();
        else
            PauseGame();
    }

    public void ReturnToMain()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");      
    }
}
