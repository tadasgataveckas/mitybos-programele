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
    public int height;
    public double weight;

    public List<string> allergies;

    string connectionLocal = "Server=localhost;User ID=root;Password=root;Database=food_db";

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
        SwitchSegment(currentSegment);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
