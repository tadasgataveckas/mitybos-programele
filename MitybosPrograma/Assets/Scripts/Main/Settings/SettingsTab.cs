using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsTab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI info;
    [SerializeField] private TextMeshProUGUI usernameText;

    private UserData userData;
    private int id_user;
    private UserCalories userCalories;
    private ClientMethods clientMethods;

    void Awake()
    {
        clientMethods = new ClientMethods(new DatabaseMethods());
        id_user = SessionManager.GetIdKey();
        userData = new UserData(id_user);
        userCalories = new UserCalories(id_user);
    }

    private void OnEnable()
    {
        userData.SynchData();
        userCalories.SynchData();


        string username = clientMethods.ReturnUsername(userData.id_user);
        usernameText.text = "User: " + username;

        info.text = $"Height: {userData.height}\n" +
                    $"Weight: {userData.weight}\n" +
                    $"Gender: {userData.GetGenderString()}\n" +
                    $"Goal: {userData.GetGoalString()}\n" +
                    $"Physical Activity: {userData.physical_activity}\n" +
                    $"Year of Birth: {userData.date_of_birth}\n" +
                    $"Creation date: {userData.creation_date.Substring(0, 10)}\n" +
                    $"BMI: {userCalories.bmi}\n" +
                    $"Daily Calories: {userCalories.calories}";

    }

    public void LogOut()
    {
        SessionManager.CloseSession();
        GoToLogin();
    }

    private void GoToLogin()
    {
        SceneManager.LoadScene("Login");
    }
}
