﻿using System;
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
    //public DatabaseMethods databaseMethods;

    // User survey info

    public TextMeshPro user;
    public TextMeshPro info;
    public TextMeshProUGUI currHeight;
    public TextMeshProUGUI currWeight;
    public TextMeshProUGUI currGender;
    public TextMeshProUGUI currGoal;
    public TextMeshProUGUI currPA;
    public TextMeshProUGUI currBirth;

    ClientMethods c = new ClientMethods(new DatabaseMethods());

    // user_data object meant for storing and retrieving info
    UserData userData;

    SurveyManager survey;
    BmiCalories bmiCalories;

    //private double bmi;
    //private double calories;
    public double BMI;
    public double CALORIES;

    public void SwitchSegment(int switchTo)
    {
        //teleport camera to position
        camera.transform.position = new Vector3(segments[switchTo].transform.position.x, 0, -10);
        currentSegment = switchTo;
        for (int i = 0; i < segmentButtons.Count; i++)
        {
            segmentButtons[i].GetComponent<ButtonTransitions>().SetSelectedSegment(i == switchTo);
        }
        camera.GetComponent<CameraScroll>().minY = segments[currentSegment].GetComponent<SegmentInformation>().minYScroll;
        camera.GetComponent<CameraScroll>().maxY = segments[currentSegment].GetComponent<SegmentInformation>().maxYScroll;
    }


    void Start()
    {
        // gets stored user id
        int id_user = SessionManager.GetIdKey();

        // retrieves user_data into object
        userData = new UserData(id_user);
        SynchUserData();
        Debug.Log(userData.ToString());

        bmiCalories = new BmiCalories();

        SwitchSegment(currentSegment);
        UpdateInfo();
    }   

    public void UpdateInfo()
    {

        user.text = "User: " + c.ReturnUsername(userData.id_user);
        c.UpdateUserData(userData);

        BMI = bmiCalories.CalculateBMI(userData.height, userData.weight);
        //Need to fix Calories
        //CALORIES = bmiCalories.CalculateDailyCalories(userData, survey.ReturnYear());
        //Debug.Log("Year is: " + survey.year);
        info.text = $"Height: {userData.height}\n" +
                    $"Weight: {userData.weight}\n" +
                    $"Gender: {userData.GetGenderString()}\n" +
                    $"Goal: {userData.GetGoalString()}\n" +
                    $"Physical Activity: {userData.physical_activity}\n" +
                    $"Date of Birth: {userData.date_of_birth}\n" +
                    $"Creation date: {userData.creation_date.Substring(0, 9)}\n" +
                    $"BMI: {BMI}";
        //$"Daily Calories: {CALORIES}";

        currHeight.text = userData.height.ToString();
        currWeight.text = userData.weight.ToString();
        currGender.text = userData.GetGenderString();
        //Goal not showing
        currGoal.text = userData.GetGoalString();
        //Debug.Log(userData.GetGoalString());
        //Not working
        currPA.text = userData.physical_activity.ToString();
        currBirth.text = userData.date_of_birth;


    }

    // created this function to be called after pressing back on settings edit
    public void SynchUserData()
    {
        userData.SynchData();
    }

    //public void CurrentHeight()
    //{
    //    currHeight.text = userData.height.ToString();
    //}

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

    
}
