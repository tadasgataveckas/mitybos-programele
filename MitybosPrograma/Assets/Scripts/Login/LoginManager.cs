using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // Login data
    public string username; // username = email?

    //public string email;

    public string password;


    public List<GameObject> segments;


    public void InputUsername(string newUsername)
    {
        username = newUsername;
    }

    public void InputPassword(string newPassword)
    {
        password = newPassword;
        // Pakeisti laukelio reiksmes i zvaigzdutes
        // ...
    }

    public void SubmitLogin() 
    {
        // ...
        SceneManager.LoadScene("Survey");
    }

    public void SubmitSignUp()
    {
        // ...
        SwitchSegment(0);
    }
    public void LoginWithAPI()
    {
        // ...
    }

    public void SwitchSegment(int switchTo)
    {
        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].SetActive(i == switchTo); // Turns on chosen segment, turns off other segments
        }
    }

    void Start()
    {
        SwitchSegment(0);
    }
}
