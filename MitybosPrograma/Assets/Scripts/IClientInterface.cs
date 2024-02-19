using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClientInterface
{
    int Login(string name, string password, string constring, out int id);
    void SendSurveyResults();
}
