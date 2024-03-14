using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderText;
    public TextMeshProUGUI descriptionText;

    SurveyManager survey;
    public void PrintSliderValue()
    {
        Debug.Log("Slider Value: " + slider.value);
        
    }

    public string ReturnActivity()
    {
        return slider.value.ToString();
    }

    void Start()
    {

    }

    void Update()
    {
        sliderText.text = slider.value.ToString();
        if (slider.value == 1)
        {
            descriptionText.text = "If you are sedentary (little or no exercise)";
            //descriptionText.text = "Group I - those in very light manual work (students, civil servants, people with mental jobs). " +
            //    "This group has a FAL of 1.2 - very low activity (sedentary or no physical activity).";
        }
        else if (slider.value == 2)
        {
            descriptionText.text = "If you are lightly active (light exercise/sports 1-3 days​/week)";
            //descriptionText.text = "Group II - light manual workers (doctors, nurses, agronomists, drivers, service workers, etc.). " +
            //    "FAL is 1.375 - low activity 1 to 3 times a week.";
        }
        else if (slider.value == 3)
        {
            descriptionText.text = "If you are moderately active (moderate exercise/sports 3-5 days/week)";
            //descriptionText.text = "Group III - those engaged in moderate physical work (surgeons, light industrial workers, managers, " +
            //    "housewives, utility workers and people engaged in mechanised manual work). FAL is 1.55 - average activity 3 to 5 times a week.";
        }
        else if (slider.value == 4)
        {
            descriptionText.text = "If you are very active (hard exercise/sports 6-7 days a week)";
            //descriptionText.text = "Group IV: those engaged in heavy manual work (athletes, farmers, construction workers and workers in " +
            //    "other industries engaged in non-mechanised and semi-mechanised work). FAL is 1.725 - high activity 6 to 7 times a week.";
        }
        else if (slider.value == 5)
        {
            descriptionText.text = "If you are extra active (very hard exercise/sports & physical job or 2x training)";
        }
    }
}
