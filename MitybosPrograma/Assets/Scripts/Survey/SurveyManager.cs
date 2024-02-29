using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    string constring = "Server=localhost;User ID=root;Password=root;Database=food_db";
    ClientMethods c = new ClientMethods(new DatabaseMethods());
    public void SubmitSurvey()
    {
        Debug.Log(LoginManager.id);
        c.UpdateProfile(LoginManager.id,gender,height,weight,goal,constring);
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
    }

    public void NextSegment() 
    {
        SwitchSegment(currentSegment + 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        //inputField.contentType = InputField.ContentType.IntegerNumber;
        SwitchSegment(currentSegment);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
