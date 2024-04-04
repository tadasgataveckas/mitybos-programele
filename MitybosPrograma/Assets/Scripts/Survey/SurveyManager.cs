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
    private UserData userData;
    private BmiCalories bmiCalories;
    private int eatingPreference = -1;
    private List<int> allergies = new List<int>();

    ClientMethods c = new ClientMethods(new DatabaseMethods());

    //activity slider
    public SliderScript slider;

    // Continue and Back buttons
    public GameObject Continue;
    public GameObject Back;

    // User survey info
    public TextMeshProUGUI info;

    // Survey errors
    public TextMeshProUGUI error;

    // Progress Bar
    public Slider ProgressBar;
    float goalSliderValue = 0;
    public float progressBarSpeed = 0.5f;

    public double BMI;
    public double CALORIES;
    public int year;


    void Start()
    {
        bmiCalories = new BmiCalories();
        SwitchSegment(currentSegment);

        // retrieves id from playerprefs
        int id = SessionManager.GetIdKey();
        userData = new UserData(id);
        // added base value cuz slider can be ignored
        userData.physical_activity = 1;
    }

    void Update()
    {
        // update progressbar
        goalSliderValue = 1f / (segments.Count - 1) * currentSegment;
        if (!Mathf.Approximately(ProgressBar.value, goalSliderValue))
        {
            ProgressBar.value = Mathf.MoveTowards(ProgressBar.value, goalSliderValue, progressBarSpeed * Time.deltaTime);
        }
    }

    public void GoToMain()
    {
        //Go to Main scene
        SceneManager.LoadScene("Main");
    }

    public void SubmitSurvey()
    {
        Debug.ClearDeveloperConsole();

        // user_data
        c.InsertUserData(userData);

        // allergies
        if (eatingPreference > 0)
            allergies.Add(eatingPreference);

        foreach (int allergy in allergies)
            c.InsertUserAllergy(userData.id_user, allergy);

        GoToMain();
    }

    // input features ----------------------------------------------------------

    public void InputGender(string newGender)
    {
        if (userData.gender == UserData.ParseGender(newGender))
            userData.gender = UserData.Gender.Null;
        else
            userData.gender = UserData.ParseGender(newGender);
    }

    public void InputGoal(string newGoal)
    {
        if (userData.goal == UserData.ParseGoal(newGoal))
            userData.goal = UserData.Goal.Null;
        else
            userData.goal = UserData.ParseGoal(newGoal);
    }

    public void InputEatingPreference(int newEatingPreference)
    {
        if (eatingPreference == newEatingPreference)
            eatingPreference = -1;
        else
            eatingPreference = newEatingPreference;
    }

    public void InputAge(string newBirthDate)
    {
        userData.date_of_birth = newBirthDate;
    }

    public void InputHeight(string newHeight)
    {
        double.TryParse(newHeight, out userData.height);
    }

    public void InputWeight(string newWeight)
    {
        double.TryParse(newWeight, out userData.weight);
    }

    public void AddAllergy(int allergy)
    {
        if (allergies.Contains(allergy)) // Check if the allergy is in list
        {
            allergies.Remove(allergy); // If yes, remove it
        }
        else
        {
            allergies.Add(allergy); // If not, add it
            Debug.Log("Added allergy: " + allergy);
        }
    }

    public string GetAllergiesAsString()
    {
        // Use StringBuilder to efficiently build the string
        StringBuilder sb = new StringBuilder();

        // Append each allergy to the string
        foreach (int allergy in allergies)
        {
            // skips vegetarianism and veganism
            if (allergy < 10)
            {
                sb.Append(Allergy.ReturnAllergyName(allergy));
                sb.Append(", "); // Add a comma and space to separate allergies
            }
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
            int.TryParse(newActivity, out userData.physical_activity);
        }
        else
        {
            Debug.LogError("Slider object is not initialized.");
        }
    }

    // Calculating features ----------------------------------------------------
    public void Year()
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
            error.text = "Please write your birth date in correct form! (yyyy-MM-dd)";
            Debug.Log("Please write your birth date in correct form! (yyyy-MM-dd)");
        }
    }
    public int ReturnYear()
    {
        return year;
    }

    // Survey Segments ---------------------------------------------------------

    // Each segment of survey (picking gender, picking eating preference, etc.)
    public List<GameObject> segments;
    private int currentSegment = 0;

    public void SwitchSegment(int switchTo)
    {
        for (int i = 0; i < segments.Count; i++)
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
        if (currentSegment == segments.Count - 1)
        {
            Back.transform.localPosition = new Vector3(0f, Back.transform.localPosition.y, 0f); ;
        }
        else { Back.transform.localPosition = new Vector3(-293f, Back.transform.localPosition.y, 0f); }
        // Turn on off
        Back.SetActive(currentSegment != 0);
        Continue.SetActive(currentSegment != segments.Count - 1);
    }

    public void NextSegment()
    {
        if (currentSegment + 1 < segments.Count)
        {
            // Checking if all data are in input
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


                string goalText = "";
                if (userData.goal == UserData.Goal.GainWeight ||
                    userData.goal == UserData.Goal.GainMuscle)
                {
                    goalText = " (added 500 kcal)";
                }
                else if (userData.goal == UserData.Goal.LoseWeight)
                {
                    goalText = " (reduced by 10%)";
                }

                BMI = bmiCalories.CalculateBMI(userData.height, userData.weight);
                CALORIES = bmiCalories.CalculateDailyCalories(userData, year);
                // Printing user survey data 
                info.text = "Your submitted info: " + "\n" +
                    "\n" +
                    "Gender: " + userData.GetGenderString() + "\n" +
                    "Goal: " + userData.GetGoalString() + "\n" +
                    "Eating preference: " + Allergy.ReturnAllergyName(eatingPreference) + "\n" +
                    "Date of birth: " + userData.date_of_birth + "\n" +
                    "Height: " + userData.height + "\n" +
                    "Weight: " + userData.weight + "\n" +
                    "Allergies: " + GetAllergiesAsString() + "\n" +
                    "Activity level: " + userData.physical_activity + "\n" +
                    "\n" +
                    "Your BMI: " + BMI + "\n" +
                    "Your BMI result: " + bmiCalories.BMIResult(BMI) + "\n" +
                    "Needed daily calories: " + CALORIES + goalText + "\n";
            }
        }
    }

    public string ReturnInfoToMain()
    {
        return info.text.ToString();
    }

    public void PreviousSegment()
    {
        SwitchSegment(currentSegment - 1);
    }

    // Checking if survey has gender
    private bool GenderEntered()
    {
        return userData.gender != UserData.Gender.Null;
    }

    // Checking if survey has eating preference
    private bool EatingEntered()
    {
        Debug.Log("EATING THING IS: " + eatingPreference);
        return eatingPreference != -1;
    }

    // Checking if survey has goal
    private bool GoalEntered()
    {
        return userData.goal != UserData.Goal.Null;
    }

    // Checking if survey parameters are correct
    private bool AHWEntered()
    {        
        return (DateTime.TryParseExact(userData.date_of_birth, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _)) &&
            (250 > userData.height && userData.height > 120) && (350 > userData.weight && userData.weight > 30);
    }
}
