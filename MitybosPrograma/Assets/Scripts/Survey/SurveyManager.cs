using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyManager : MonoBehaviour
{
    // Physical Data

    public string gender;

    public string goal;

    public string eatingPreference;
   
    //Kolkas string reikia suzinot kaip paversti i int ir double
    public string age;
    public string height;
    public string weight;

    public List<string> allergies;

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
        age = newAge;
        Debug.Log("You Entered " + age);
    }
    public void InputHeight(string newHeight)
    {
        height = newHeight;
    }
    public void InputWeight(string newWeight)
    {
        weight = newWeight;
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
