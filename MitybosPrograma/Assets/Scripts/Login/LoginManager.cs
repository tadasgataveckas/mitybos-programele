using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // Login data
    public string username; // username = email? 

    //public string email;

    public string password;

    public TextMeshProUGUI errorAcc;

    public TextMeshProUGUI errorData;

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
        // Validating with username and password 
        if (username.Length == 0 && password.Length == 0)
        {
            errorData.text = "You need to input your username and password!";
        }
        else if (username.Length == 0)
        {
            errorData.text = "You need to input your username!";
        }
        else if (password.Length == 0)
        {
            errorData.text = "You need to input your password!";
        }
        else if (username.Length <= 4 && password.Length <= 4)
        {
            errorData.text = "Your username and password are too short, need to be at least 5!";
        }
        else if (password.Length <= 4)
        {
            errorData.text = "Your password is too short, at least 5!";
        }
        else if (username.Length <= 4)
        {
            errorData.text = "Your username is too short, at least 5!";
        }
        else
        {
            int id = c.Login(username, password, out id, constring);

            SetID(id);
            if (id > 0)
            {

                if (c.CheckSurveyCompleted(id, constring))
                {
                    SceneManager.LoadScene("Main");
                }
                else
                    SceneManager.LoadScene("Survey");
            }
            else
                // If there is no acc with provided data, back to first page + err
                SwitchSegment(0);
            errorAcc.text = "There is no account with this data, please create it!";
        }
    }


    public void SubmitSignUp()
    {
        // Validating with username and password
        if (username.Length == 0 && password.Length == 0)
        {
            errorData.text = "You need to input your username and password!";
        }
        else if (username.Length == 0)
        {
            errorData.text = "You need to input your username!";
        }
        else if (password.Length == 0)
        {
            errorData.text = "You need to input your password!";
        }
        else if (username.Length <= 4 && password.Length <= 4)
        {
            errorData.text = "Your username and password are too short, need to be at least 5!";
        }
        else if (password.Length <= 4)
        {
            errorData.text = "Your password is too short, at least 5!";
        }
        else if (username.Length <= 4)
        {
            errorData.text = "Your username is too short, at least 5!";
        }
        else
        {
            // If everything correct, success and survey
            c.Register(username, username, password, constring);           
            SceneManager.LoadScene("Survey");
        }       
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
            errorAcc.text = "";
            errorData.text = "";
        }
    }

    void Start()
    {
        SwitchSegment(0);
    }
}
