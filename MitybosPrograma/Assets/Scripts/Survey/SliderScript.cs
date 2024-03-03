using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderText;
    // Start is called before the first frame update
    void Start()
    {
        //slider.onValueChanged.AddListener((v) =>
        //{
        //    sliderText.text = v.ToString("0.00");

        //});
    }

    // Update is called once per frame
    void Update()
    {
        sliderText.text = slider.value.ToString();

    }
}
