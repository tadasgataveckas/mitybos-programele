using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClientInterface
{
    bool Login(string name, string password);
    void SendSurveyResults();
}
