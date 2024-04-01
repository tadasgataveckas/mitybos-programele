using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClientInterface
{
    int Login(string name, string password, out int id);
    bool RegisterUser(string email, string name, string password);

    void InsertUserData(UserData userData);
    void UpdateUserData(UserData userData);

    bool CheckIfSurveyCompleted(int id);

    string ReturnUsername(int id);

    List<FoodClass> ReturnFoodList();
    bool IsUsernameTaken(string username);
}
