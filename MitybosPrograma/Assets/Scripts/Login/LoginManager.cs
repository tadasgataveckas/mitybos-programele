using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    string constring = "Server=localhost;User ID=root;Password=root;Database=food_db";
    
    ClientMethods c = new ClientMethods(new DatabaseMethods());
    public static int id; //iki kol pakeista i string

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

    public void SetID(int newid)
    {
        id = newid;
    }


    public void SubmitLogin() 
    {
        bool completed;
        int id = c.Login(username, password, out id,constring);
        
        SetID(id);

        if (id > 0) {
            if (c.CheckSurveyCompleted(id, constring))
            {
                SceneManager.LoadScene("Main");
            }
            else
                SceneManager.LoadScene("Survey");
        }
        else
            SwitchSegment(0);
    }

    public void SubmitSignUp()
    {
        c.Register(username,username, password,constring);
       
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
