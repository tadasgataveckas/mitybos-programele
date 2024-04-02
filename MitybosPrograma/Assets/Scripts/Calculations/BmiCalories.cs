using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BmiCalories : MonoBehaviour
{
    SurveyManager survey;
    //UserData userData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Calculating BMI
    public double CalculateBMI(double Bheight, double Bweight)
    {
        double BMI = Bweight / Math.Pow(Bheight, 2);
        return Math.Round(BMI * 10000, 2);
    }

    //Printing BMI result
    public string BMIResult(double RBMI)
    {
        if (RBMI < 16.0)
        {
            return "Severe Underweight";
        }
        else if (16.0 < RBMI && RBMI < 17.0)
        {
            return "Moderate Underweight";
        }
        else if (17.0 < RBMI && RBMI < 18.5)
        {
            return "Mild Underweight";
        }
        else if (18.5 < RBMI && RBMI < 25.0)
        {
            return "Normal weight";
        }
        else if (25.0 < RBMI && RBMI < 30.0)
        {
            return "Overweight";
        }
        else if (30.0 < RBMI && RBMI < 35.0)
        {
            return "Obese Class I";
        }
        else if (35.0 < RBMI && RBMI < 40.0)
        {
            return "Obese Class II";
        }
        else if (RBMI > 40.0)
        {
            return "Obese Class III";
        }
        return "";
    }

    //Calculating daily calories
    public double CalculateDailyCalories(UserData userData, int year)
    {
        // The Basal Metabolic Rate
        double BMR = 0;

        //physical activity level
        double FAL = 0;

        switch (userData.physical_activity)
        {
            case 1:
                FAL = 1.2;
                break;
            case 2:
                FAL = 1.375;
                break;
            case 3:
                FAL = 1.55;
                break;
            case 4:
                FAL = 1.725;
                break;
            case 5:
                FAL = 1.9;
                break;
        }

        int currentYear = DateTime.Now.Year;
        if (userData.gender == UserData.Gender.Female)
            BMR = (10 * userData.weight) + (6.25 * userData.height) - (5 * (currentYear - year)) - 161;
        else
            BMR = (10 * userData.weight) + (6.25 * userData.height) - (5 * (currentYear - year)) + 5;

        // Daily calories = metabolism * physical activity level
        double CALORIES = BMR * FAL;

        // Daily calories adjusted on user goal 
        if (userData.goal == UserData.Goal.LoseWeight)
        {
            CALORIES = CALORIES * 0.9;
        }
        else if (userData.goal == UserData.Goal.GainWeight ||
                 userData.goal == UserData.Goal.GainMuscle)
        {
            CALORIES = CALORIES + 500;
        }
        return Math.Round(CALORIES, 2);
    }
}
