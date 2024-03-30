using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
            {
                // If there is no acc with provided data, back to first page + err
                SwitchSegment(0);
                errorAcc.text = "There is no account with this data, please create it!";
            }
        }
    }


    public void SubmitSignUp()
    {
        // Validating username and password
        if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
        {
            errorData.text = "You need to input your username and password!";
        }
        else if (string.IsNullOrEmpty(username))
        {
            errorData.text = "You need to input your username!";
        }
        else if (string.IsNullOrEmpty(password))
        {
            errorData.text = "You need to input your password!";
        }
        else if (username.Length <= 4 && password.Length <= 4)
        {
            errorData.text = "Your username and password are too short, they need to be at least 5 characters long!";
        }
        else if (password.Length <= 4)
        {
            errorData.text = "Your password is too short, it needs to be at least 5 characters long!";
        }
        else if (username.Length <= 4)
        {
            errorData.text = "Your username is too short, it needs to be at least 5 characters long!";
        }
        else
        {
            // Check if user  with username already exists in the database
            bool userExists = c.CheckIfUserExists(username, constring);

            if (userExists)
            {
                errorData.text = "User with this username already exists!";
            }
            else
            {
                // If everything is correct, register the user, log them in, and proceed to the survey
                c.Register(username, username, password, constring);
                int id = c.Login(username, password, out id, constring);
                SetID(id);
                SceneManager.LoadScene("Survey");
            }
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
