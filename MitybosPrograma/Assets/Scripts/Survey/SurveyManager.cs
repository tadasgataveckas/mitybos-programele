using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private double CALORIES;


    string constring = "Server=localhost;User ID=root;Password=root;Database=food_db";
    ClientMethods c = new ClientMethods(new DatabaseMethods());
    public void SubmitSurvey()
    {
        Debug.Log(LoginManager.id);
        c.UpdateProfile(LoginManager.id,gender,height,weight,goal,age,activity,constring);
        GoToMain();
    }

    public void InputGender(string newGender) 
    {
        if (newGender == gender)
        {
            gender = "";
        }
        else
        {
            gender = newGender;
        }
    }

    public void InputGoal(string newGoal)
    {
        if (newGoal == goal)
        {
            goal = "";
        }
        else
        {
            goal = newGoal;
        }
    }

    public void InputEatingPreference(string newEatingPreference)
    {
        if (newEatingPreference == eatingPreference)
        {
            eatingPreference = "";
        }
        else
        {
            eatingPreference = newEatingPreference;
        }
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

    public string GetAllergiesAsString()
    {
        // Use StringBuilder to efficiently build the string
        StringBuilder sb = new StringBuilder();

        // Append each allergy to the string
        foreach (string allergy in allergies)
        {
            sb.Append(allergy);
            sb.Append(", "); // Add a comma and space to separate allergies
        }

        // Remove the last comma and space
        if (sb.Length > 0)
        {
            sb.Length -= 2;
        }

        // Return the concatenated string
        return sb.ToString();
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
        return Math.Round(BMI * 10000, 2);
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
    public double CalculateDailyCalories()
    {
        // The Basal Metabolic Rate
        double BMR = 0;

        //physical activity level
        double FAL = 0;

        if (activity == 1)
        {
            FAL = 1.2;
        }
        else if (activity == 2)
        {
            FAL = 1.375;
        }
        else if (activity == 3)
        {
            FAL = 1.55;
        }
        else if (activity == 4)
        {
            FAL = 1.725;
        }
        else if (activity == 5)
        {
            FAL = 1.9;
        }

        if (gender == "Woman")
        {
            //pakeisti 2024 i time now
            BMR = (10 * weight) + (6.25 * height) - (5 * (2024-age)) - 161;

        }
        else
        {
            BMR = (10 * weight) + (6.25 * height) - (5 * (2024 - age)) + 5;
        }

        //daily calories = metabolism * physical activity level
        CALORIES = BMR * FAL;
        return Math.Round(CALORIES,2);
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

    //Old NextSegment
    //public void NextSegment() 
    //{
    //    SwitchSegment(currentSegment + 1);
    //}

    // User survey info
    public TextMeshProUGUI info;

    // Survey errors
    public TextMeshProUGUI error;

    public void NextSegment()
    {
        if (currentSegment + 1 < segments.Count)
        {
            // Tikriname, ar visi reikiami duomenys yra įvesti
            if (currentSegment == 0 && !GenderEntered())
            {
                Debug.Log("Please fill in all required fields.");
                error.text = "Please choose your gender to continue the survey!";
            }
            else if (currentSegment == 1 && !EatingEntered())
            {
                Debug.Log("Please fill in all required fields.");
                error.text = "Please choose your eating preference to continue the survey!";
            }
            else if (currentSegment == 2 && !GoalEntered())
            {
                Debug.Log("Please fill in all required fields.");
                error.text = "Please choose your goal to continue the survey!";
            }
            else if (currentSegment == 3 && !AHWEntered())
            {
                Debug.Log("Please fill in all required fields.");
                error.text = "Please write all your physical data in correct form!";
            }
            else
            {
                SwitchSegment(currentSegment + 1);
                // Išvalome klaidų pranešimus, jei vartotojas tęsia į kitą segmentą
                error.text = "";

                double bmi = CalculateBMI(height, weight);
                //double calories = CalculateDailyCalories(activity, gender, age, weight);
                // Printing user survey data 
                info.text = "Your submitted info: " + "\n" +
                    "\n" +
                    "Gender: " + gender + "\n" +
                    "Goal: " + goal + "\n" +
                    "Eating preference: " + eatingPreference + "\n" +
                    "Age: " + age + "\n" +
                    "Height: " + height + "\n" +
                    "Weight: " + weight + "\n" +
                    "Allergies: " + GetAllergiesAsString() + "\n" +
                    "Activity level: " + activity + "\n" +
                    "\n" +
                    "Your BMI: " + bmi + "\n" +
                    "Your BMI result: " + BMIResult(bmi) + "\n" +
                    "Needed daily calories: " + CalculateDailyCalories() + "\n";
            }
        }
    }


    public void PreviousSegment()
    {
        SwitchSegment(currentSegment - 1);       
    }

    // Checking if survey has gender
    private bool GenderEntered()
    {
        return !string.IsNullOrEmpty(gender);
    }

    // Checking if survey has goal
    private bool EatingEntered()
    {
        return !string.IsNullOrEmpty(eatingPreference);
    }

    // Checking if survey has goal
    private bool GoalEntered()
    {
        return !string.IsNullOrEmpty(goal);
    }

    // Checking if survey has goal
    private bool AHWEntered()
    {
        return 2023 > age && age > 1960  &&  250 > height && height > 120  &&  350 > weight && weight > 30;
    }


    //public void PreviousSegment()
    //{
    //    SwitchSegment(currentSegment - 1);
    //}

    void Start()
    {
        SwitchSegment(currentSegment);
    }
    
    void GoToMain() {
        //Go to Main scene
        SceneManager.LoadScene("Main");
    }

    // Progress Bar
    public Slider ProgressBar;
    float goalSliderValue = 0;
    public float progressBarSpeed = 0.5f;
    void Update()
    {
        goalSliderValue = 1f / (segments.Count - 1) * currentSegment;
        if(!Mathf.Approximately(ProgressBar.value, goalSliderValue)) 
        {
            ProgressBar.value = Mathf.MoveTowards(ProgressBar.value, goalSliderValue, progressBarSpeed * Time.deltaTime);
        }
    }
}
