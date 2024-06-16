using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeTab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dailyCalories;
    [SerializeField] private Slider progressBar;

    private UserCalories userCalories;
    private UserData userData;
    private int id_user;
    ClientMethods clientMethods;

    // Start is called before the first frame update
    void Awake()
    {
        clientMethods = new ClientMethods(new DatabaseMethods());
        id_user = SessionManager.GetIdKey();
        userData = new UserData(id_user);
        userCalories = new UserCalories(id_user);
    }

    private void OnEnable()
    {
        userCalories.SynchData();
        float calories = clientMethods.GetTotalKcalFromDate(id_user, DateTime.Today.ToString("yyyy-MM-dd"));
        dailyCalories.text = calories + " / " + userCalories.calories + " kcal";

        float percentage = 0;
        if (userCalories.calories > 0)
            percentage = calories / userCalories.calories;
        progressBar.value = Math.Clamp(percentage, 0, 1);
    }
}
