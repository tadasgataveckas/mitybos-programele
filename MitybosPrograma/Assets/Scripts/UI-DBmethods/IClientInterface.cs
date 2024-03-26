using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClientInterface
{
    int Login(string name, string password, out int id, string constring);
    bool Register(string email, string name, string password, string constring);

    void InsertRegisterPlaceholder(int id, string constring);
    void UpdateProfile(int id, string gender, double height, double weight, string goal, int dateOfBirth, int activity, string constring);

    bool CheckSurveyCompleted(int id, string constring);

    string ReturnUserData(int id, string constring);

    string ReturnUsername(int id, string constring);

    List<FoodClass> ReturnFoodList(string constring);
}
