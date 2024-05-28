using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    public bool portraitOrientation = true;

    void Start()
    {
        if (portraitOrientation)
        {
            Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
        }
    }
}
