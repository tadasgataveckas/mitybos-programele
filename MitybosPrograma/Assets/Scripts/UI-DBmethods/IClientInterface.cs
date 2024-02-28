using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClientInterface
{
    int Login(string name, string password, out int id, string constring);
    bool Register(string email, string name, string password, string constring);
    void UpdateProfile(int id, string gender, double height, double weight, string goal, string constring);
}
