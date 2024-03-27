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

    public DatabaseMethods databaseMethods;

    string constring = "Server=localhost;User ID=root;Password=root;Database=food_db";
    // User survey info

    public TextMeshPro user;
    public TextMeshPro info;

    ClientMethods c = new ClientMethods(new DatabaseMethods());

    public static int id;

    // User data for editing
    public double height;
    public double weight;
    public string gender;
    public string goal;
    public int activity;
    public string birth;

    //int currentYear = DateTime.Now.Year;

    public void SwitchSegment(int switchTo)
    {
        //teleport camera to position
        camera.transform.position = new Vector3(segments[switchTo].transform.position.x, 0, -10);
        currentSegment = switchTo;
        for(int i = 0; i < segmentButtons.Count;i++)
        {
            segmentButtons[i].GetComponent<ButtonTransitions>().SetSelectedSegment(i == switchTo);
        }
        camera.GetComponent<CameraScroll>().minY = segments[currentSegment].GetComponent<SegmentInformation>().minYScroll;
        camera.GetComponent<CameraScroll>().maxY = segments[currentSegment].GetComponent<SegmentInformation>().maxYScroll;
    }


    public TextMeshProUGUI currHeight;

    void Start()
    {
        SwitchSegment(currentSegment);
        user.text = "User: " + c.ReturnUsername(LoginManager.id, constring);
        string userDataString = c.ReturnUserData(LoginManager.id, constring);
        string[] userDataParts = userDataString.Split(';');
        info.text = $"Height: {userDataParts[0]}\n" +
                    $"Weight: {userDataParts[1]}\n" +
                    $"Gender: {userDataParts[2]}\n" +
                    $"Goal: {userDataParts[3]}\n" +
                    $"Physical Activity: {userDataParts[4]}\n" +
                    $"Date of Birth: {userDataParts[5]}\n" +
                    $"Creation Date: {userDataParts[6]}";

        currHeight.text = userDataParts[0]; 
        double.TryParse(userDataParts[0], out height);
        double.TryParse(userDataParts[1], out weight);
        gender = userDataParts[2];
        goal = userDataParts[3];
        int.TryParse(userDataParts[4], out activity);
        birth = userDataParts[5];
        Debug.Log("height is: " + height);
        Debug.Log("weight is: " + weight);
        Debug.Log("gender is: " + gender);
        Debug.Log("goal is: " + goal);
        Debug.Log("phy ac is: " + activity);
        Debug.Log("birth is: " + birth);
        //height = userDataParts[0];
        //weight = userDataParts[1];
        //gender = userDataParts[2];
        //goal = userDataParts[3];
        //activity = userDataParts[4];
        //age = userDataParts[5];
    }

    public void UpdateInfo()
    {
        user.text = "User: " + c.ReturnUsername(LoginManager.id, constring);

        info.text = $"Height: {height}\n" +
                    $"Weight: {weight}\n" +
                    $"Gender: {gender}\n" +
                    $"Goal: {goal}\n" +
                    $"Physical Activity: {activity}\n" +
                    $"Date of Birth: {birth}\n";
        //$"Creation Date: {userDataParts[6]}";
        c.UpdateProfile(LoginManager.id, gender, height, weight, goal, birth, activity, constring);
    }

    
    



    public void InputHeight(string newHeight)
    {
        if (double.TryParse(newHeight, out height))
        {
            Debug.Log("Edited height is: " + height);
        }
    }

    public void InputWeight(string newWeight)
    {
        if (double.TryParse(newWeight, out weight))
        {
            Debug.Log("Edited weight is: " + weight);
        }
    }

    public void InputGender(int val)
    {
        if (val == 0)
        {
            gender = "Male";
            Debug.Log("Edited gender is: " + gender);
        }
        if (val == 1)
        {
            gender = "Female";
            Debug.Log("Edited gender is: " + gender);
        }
        if (val == 2)
        {
            gender = "Other";
            Debug.Log("Edited gender is: " + gender);
        }
        
    }

    public void InputGoal(int val1)
    {
        if (val1 == 0)
        {
            goal = "Lose weight";
            Debug.Log("Edited goal is: " + goal);
        }
        if (val1 == 1)
        {
            goal = "Gain weight";
            Debug.Log("Edited goal is: " + goal);
        }
        if (val1 == 2)
        {
            goal = "Gain muscle";
            Debug.Log("Edited goal is: " + goal);
        }
        if (val1 == 3)
        {
            goal = "Maintain weight";
            Debug.Log("Edited goal is: " + goal);
        }
    }

    public void InputActivity(int val2)
    {
        if (val2 == 0)
        {
            activity = 1;
            Debug.Log("Edited activity is: " + activity);
        }
        if (val2 == 1)
        {
            activity = 2;
            Debug.Log("Edited activity is: " + activity);
        }
        if (val2 == 2)
        {
            activity = 3;
            Debug.Log("Edited activity is: " + activity);
        }
        if (val2 == 3)
        {
            activity = 4;
            Debug.Log("Edited activity is: " + activity);
        }
        if (val2 == 4)
        {
            activity = 5;
            Debug.Log("Edited activity is: " + activity);
        }
    }

    public void InputBirth(string newBirth)
    {
        birth = newBirth;
        Debug.Log("Edited birth is: " + birth);
    }
}
