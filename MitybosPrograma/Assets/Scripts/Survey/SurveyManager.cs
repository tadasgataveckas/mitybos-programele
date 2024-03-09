using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SurveyManager : MonoBehaviour
{
    // Physical Data

    public string gender;

    public string goal;

    public string eatingPreference;
   
    public int age;

    public double height;

    public double weight;

    public List<string> allergies;

    public int activity;

    //activity slider
    public SliderScript slider;

    // Continue and Back buttons

    public GameObject Continue;
    public GameObject Back;


    private double BMI;


    string constring = "Server=localhost;User ID=root;Password=root;Database=food_db";
    ClientMethods c = new ClientMethods(new DatabaseMethods());
    public void SubmitSurvey()
    {
        Debug.Log(LoginManager.id);
        c.UpdateProfile(LoginManager.id,gender,height,weight,goal,age,activity,constring);
    }

    public void InputGender(string newGender) 
    {
        gender = newGender;
    }

    public void InputGoal(string newGoal)
    {
        goal = newGoal;
    }

    public void InputEatingPreference(string newEatingPreference)
    {
        eatingPreference = newEatingPreference;
    }

    public void InputAge(string newAge)
    {
        if (int.TryParse(newAge, out age))
        {
            Debug.Log("Input age is: " + age);
        }
    }

    public void InputHeight(string newHeight)
    {
        if (double.TryParse(newHeight, out height))
        {
            Debug.Log("Input height is: " + height);
        }
    }

    public void InputWeight(string newWeight)
    {
        if (double.TryParse(newWeight, out weight))
        {
            Debug.Log("Input weight is: " + weight);
        }
    }

    public void AddAllergy(string allergy)
    {
        if (allergies.Contains(allergy)) // Patikriname, ar alergija jau yra sąraše
        {
            allergies.Remove(allergy); // Jei taip, pašaliname ją
        }
        else
        {
            allergies.Add(allergy); // Jei ne, pridedame ją
            Debug.Log("Added allergy: " + allergy);
        }
    }

    public void InputActivity()
    {
        if (slider != null)
        {
            string newActivity = slider.ReturnActivity();
            Debug.Log("Activity: " + newActivity);
            if (int.TryParse(newActivity, out activity))
            {
                Debug.Log("Input activity is: " + activity);
            }
        }
        else
        {
            Debug.LogError("Slider object is not initialized.");
        }
    }



    //Calculating features

    //Calculating BMI
    public double CalculateBMI(double Bheight, double Bweight)
    {
        BMI = Bweight / Math.Pow(Bheight,2);
        return BMI;
    }

    //Printing BMI result
    public string BMIResult(double RBMI)
    {
        if(RBMI < 18.5)
        {
            return "Underweight";
        }
        else if (18.5 < RBMI && RBMI < 25.0)
        {
            return "Normal weight";
        }
        else if (25.0 < RBMI && RBMI < 30.0)
        {
            return "Overweight";
        }
        else if (RBMI > 30.0)
        {
            return "Obese";
        }
        return "";
    }

    //Calculating daily calories
    public double CalculateDailyCalories(int activityLevel, string cGender, int cAge, double cWeight)
    {
        //daily calories
        double dCalories = 0.0;

        //basic metabolism
        double PMA = 0.0;

        //physical activity level
        double FAL = 0.0;

        if (activityLevel == 1)
        {
            FAL = 1.2;
        }
        else if (activityLevel == 2)
        {
            FAL = 1.375;
        }
        else if (activityLevel == 3)
        {
            FAL = 1.55;
        }
        else if (activityLevel == 4)
        {
            FAL = 1.725;
        }

        if(cGender == "Woman")
        {
            if (cAge < 30)
            {
                PMA = (0.0615 * cWeight + 2.08) * 240;
            }
            else if(cAge < 60)
            {
                PMA = (0.0364 * cWeight + 3.47) * 240;
            }
        }
        else
        {
            if (cAge < 30)
            {
                PMA = (0.064 * cWeight + 2.84) * 240;
            }
            else if (cAge < 60)
            {
                PMA = (0.0485 * cWeight + 3.67) * 240;
            }
        }

        //daily calories = metabolism * physical activity level
        dCalories = PMA * FAL;
        return dCalories;
    }



    // Survey Segments

    public List<GameObject> segments; // Each segment of survey (picking gender, picking eating preference, etc.)
    private int currentSegment = 0;

    public void SwitchSegment(int switchTo) 
    {
        for(int i=0;i<segments.Count;i++)
        {
            segments[i].SetActive(i == switchTo); // Turns on chosen segment, turns off other segments
        }
        currentSegment = switchTo;

        // Change button positions
        //First Segment
        if (currentSegment == 0)
        {
            Continue.transform.localPosition = new Vector3(0f, Continue.transform.localPosition.y, 0f);
        }
        else { Continue.transform.localPosition = new Vector3(81f, Continue.transform.localPosition.y, 0f); }
        // Last segment
        if (currentSegment == segments.Count-1) 
        {
            Back.transform.localPosition = new Vector3(0f, Back.transform.localPosition.y, 0f); ;
        }
        else { Back.transform.localPosition = new Vector3(-293f, Back.transform.localPosition.y, 0f); }
        // Turn on off
        Back.SetActive(currentSegment != 0);
        Continue.SetActive(currentSegment != segments.Count-1);

    }

    public void NextSegment() 
    {
        SwitchSegment(currentSegment + 1);
    }
    public void PreviousSegment()
    {
        SwitchSegment(currentSegment - 1);
    }


    // Start is called before the first frame update
    void Start()
    {
        SwitchSegment(currentSegment);
    }
    

    // Update is called once per frame
    void Update()
    {
       
    }
}
