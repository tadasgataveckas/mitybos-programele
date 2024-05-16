using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LoginManager : MonoBehaviour
{
    // Login data
    private string username;
    private string email;
    private string password;
    private string passwordConfirm;

    public TextMeshProUGUI errorAcc;
    public TextMeshProUGUI errorData;
    public List<GameObject> segments;

    ClientMethods c = new ClientMethods(new DatabaseMethods());
    int id;

    public void InputEmail(string newEmail)
    {
        email = newEmail;
    }

    public void InputUsername(string newUsername)
    {
        username = newUsername;
    }

    public void InputPassword(string newPassword)
    {
        password = newPassword;
    }

    public void InputPasswordConfirm(string newPasswordConfirm)
    {
        passwordConfirm = newPasswordConfirm;
    }

    public void SubmitLogin()
    {
        // Validating with username and password 
        if (string.IsNullOrEmpty(username))
        {
            errorData.text = "You need to input a username!";
        }
        else if (string.IsNullOrEmpty(password))
        {
            errorData.text = "You need to input a password!";
        }
        else if (username.Length <= 4)
        {
            errorData.text = "Username must be at least 5 characters long!";
        }
        else if (password.Length <= 4)
        {
            errorData.text = "Password must be at least 5 characters long!";
        }
        else if (!c.IsUsernameTaken(username))
        {
            errorData.text = "User does not exist!";
        }
        else if (!c.IsPasswordCorrect(username, password))
        {
            errorData.text = "Incorrect password!";
        }
        else
        {
            // logs in
            id = c.Login(username, password);

            // stores id_user in playerprefs as session id
            SessionManager.StoreIdKey(id);

            // goes to survey if it's not completed yet
            if (c.CheckIfSurveyCompleted(id))
                SceneManager.LoadScene("Main");
            else
                SceneManager.LoadScene("Survey");
        }
    }

    // Uses regex to check for email format
    private bool IsEmailValid(string email_string)
    {
        string pattern = @"[a-zA-Z0-9_\-\.]+[@][a-zA-Z0-9]+\.[a-zA-Z]{2,3}";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(email_string);
        return match.Success;
    }

    public void SubmitSignUp()
    {
        // Validating username and password
        if (string.IsNullOrEmpty(email))
        {
            errorData.text = "Please input an email address!";
        }
        else if (string.IsNullOrEmpty(username))
        {
            errorData.text = "Please input a username!";
        }
        else if (string.IsNullOrEmpty(password))
        {
            errorData.text = "Please input a password!";
        }
        else if (string.IsNullOrEmpty(passwordConfirm))
        {
            errorData.text = "Please input password confirmation!";
        }

        else if (!IsEmailValid(email))
        {
            errorData.text = "This email address is not valid!";
        }
        else if (c.IsEmailInUse(email))
        {
            errorData.text = "This email is already in use!";
        }

        else if (username.Length <= 4)
        {
            errorData.text = "Username must be at least 5 characters long!";
        }
        else if (c.IsUsernameTaken(username))
        {
            errorData.text = "This username is already in use!";
        }

        else if (password.Length <= 4)
        {
            errorData.text = "Password must be at least 5 characters long!";
        }
        else if (password != passwordConfirm)
        {
            errorData.text = "Password does not match password confirmation!";
        }
        else
        {
            if (c.RegisterUser(email, username, password))
            {
                id = c.Login(username, password);
                c.InsertUserLevelCoins(id, 0, 0, 0);
                Debug.Log("Account successfully created");
            }
            else
                Debug.Log("Account creation failed");

            // TO DO: show account creation success pop up
            //
            //

            SceneManager.LoadScene("Login");
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
        DBManager.CreateDatabase();
    }
}
