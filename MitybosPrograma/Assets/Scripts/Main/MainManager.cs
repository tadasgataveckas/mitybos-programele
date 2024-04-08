using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MainManager : MonoBehaviour
{
    public GameObject camera;
    public List<GameObject> segments;
    public List<GameObject> segmentButtons;
    public int currentSegment = 0;

    // Current User survey info in the edit fields
    public TextMeshProUGUI user;
    public TextMeshProUGUI info;

    public TextMeshProUGUI currHeight;
    public TextMeshProUGUI currWeight;
    public TextMeshProUGUI currGender;
    public TextMeshProUGUI currGoal;
    public TextMeshProUGUI currPA;
    public TextMeshProUGUI currBirth;
    public TextMeshProUGUI dailyCalories;

    ClientMethods c = new ClientMethods(new DatabaseMethods());

    // user_data object meant for storing and retrieving info
    UserData userData;

    // bmiCalories - class that calculates user's bmi index and number of daily calories
    BmiCalories bmiCalories;
    public double BMI;
    public static double CALORIES;
    int year;

    public GameObject food;
    public float currCalories;    

    public void SwitchSegment(int switchTo)
    {
        //teleport camera to position
        //camera.transform.position = new Vector3(segments[switchTo].transform.position.x, 0, -10);

        //changed for canvas
        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].SetActive(i == switchTo); // Turns on chosen segment, turns off other segments
        }

        currentSegment = switchTo;
        for (int i = 0; i < segmentButtons.Count; i++)
        {
            segmentButtons[i].GetComponent<ButtonTransitions>().SetSelectedSegment(i == switchTo);
        }
        //camera.GetComponent<CameraScroll>().minY = segments[currentSegment].GetComponent<SegmentInformation>().minYScroll;
        //camera.GetComponent<CameraScroll>().maxY = segments[currentSegment].GetComponent<SegmentInformation>().maxYScroll;
    }

    void Start()
    {
        SwitchSegment(currentSegment);

        // gets stored user id
        int id_user = SessionManager.GetIdKey();

        // retrieves user_data into object
        userData = new UserData(id_user);
        SynchUserData();
        Debug.Log(userData.ToString());

        bmiCalories = new BmiCalories();
        food.GetComponent<FoodSearch>();       

        UpdateInfo();
    }   

    public void UpdateInfo()
    {
        user.text = "User: " + c.ReturnUsername(userData.id_user);
        c.UpdateUserData(userData);

        // Bmi and daily calories
        BMI = bmiCalories.CalculateBMI(userData.height, userData.weight);
        CALORIES = bmiCalories.CalculateDailyCalories(userData, Year());

        Debug.Log("Year is: " + Year());
        info.text = $"Height: {userData.height}\n" +
                    $"Weight: {userData.weight}\n" +
                    $"Gender: {userData.GetGenderString()}\n" +
                    $"Goal: {userData.GetGoalString()}\n" +
                    $"Physical Activity: {userData.physical_activity}\n" +
                    $"Date of Birth: {userData.date_of_birth}\n" +
                    $"Creation date: {userData.creation_date.Substring(0, 9)}\n" +
                    $"BMI: {BMI}\n" +
                    $"Daily Calories: {CALORIES}";

        currHeight.text = userData.height.ToString();
        currWeight.text = userData.weight.ToString();
        currGender.text = userData.GetGenderString();
        //Goal and PA not working
        currGoal.text = userData.GetGoalString();        
        currPA.text = userData.physical_activity.ToString();
        currBirth.text = userData.date_of_birth;

        currCalories = food.GetComponent<FoodSearch>().ReturnTotalKcal();
        Debug.Log("Curr Calories: " + currCalories);
        dailyCalories.text = currCalories + " / " + CALORIES + " cal";
    }

    // created this function to be called after pressing back on settings edit
    public void SynchUserData()
    {
        userData.SynchData();
    }   

    public void InputHeight(string newHeight)
    {
        if (double.TryParse(newHeight, out userData.height))
        {
            Debug.Log("Edited height is: " + userData.height);
        }
    }

    public void InputWeight(string newWeight)
    {
        if (double.TryParse(newWeight, out userData.weight))
        {
            Debug.Log("Edited weight is: " + userData.weight);
        }
    }

    public void InputGender(int val)
    {
        userData.gender = (UserData.Gender)(val + 1);
        Debug.Log("Edited gender is: " + userData.gender);
    }

    public void InputGoal(int val1)
    {
        userData.goal = (UserData.Goal)(val1 + 1);
        Debug.Log("Edited goal is: " + userData.goal);
    }

    public void InputActivity(int val2)
    {
        userData.physical_activity = (val2 + 1);
        Debug.Log("Edited physical activity is: " + userData.physical_activity);
    }

    public void InputBirth(string newBirth)
    {
        userData.date_of_birth = newBirth;
        Debug.Log("Edited birth is: " + userData.date_of_birth);
    }

    /// <summary>
    /// Transfering from birth date to year
    /// </summary>
    /// <returns></returns>
    public int Year()
    {
        // Konvertuojame string'ą į DateTime objektą
        DateTime dataObj;
        if (DateTime.TryParseExact(userData.date_of_birth, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dataObj))
        {
            // Ištraukiame metus
            year = dataObj.Year;

            // Spausdiname metus
            Debug.Log("Konvertuoti metai: " + year);
        }
        else
        {         
            Debug.Log("Please write your birth date in correct form! (yyyy-MM-dd)");
        }
        return year;
    }

    
}
