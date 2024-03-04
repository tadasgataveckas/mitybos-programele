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
    public void PrintSliderValue()
    {
        Debug.Log("Slider Value: " + slider.value);
    }

    void Start()
    {

    }

    void Update()
    {
        sliderText.text = slider.value.ToString();
        if (slider.value == 1)
        {
            descriptionText.text = "Low";
        }
        else if (slider.value == 2)
        {
            descriptionText.text = "Medium";
        }
        else if (slider.value == 3)
        {
            descriptionText.text = "High";
        }
        else if (slider.value == 4)
        {
            descriptionText.text = "Rare";
        }
        else if (slider.value == 5)
        {
            descriptionText.text = "Super";
        }
    }
}
