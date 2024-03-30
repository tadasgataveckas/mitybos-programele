using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In progress
public class UserData
{
    public int id_user;
    public double weight;
    public double height;
    public Gender gender;
    public string goal;
    public string physical_activity;
    public int date_of_birth;
    public string creation_date;

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }
    public enum Goal
    {
        LoseWeight = 1,
        MaintainWeight = 2, 
        GainWeight = 3,
        GainMuscle = 4
    }

}
