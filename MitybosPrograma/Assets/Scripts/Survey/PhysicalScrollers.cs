using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PhysicalScrollers : MonoBehaviour
{
    //Height scroller 
    public ScrollRect scrollRectHeight;
    public Transform numberParentHeight;
    public GameObject numberPrefabHeight;
    float valueHeight;

    //Weight scroller
    public ScrollRect scrollRectWeight;
    public Transform numberParentWeight;
    public GameObject numberPrefabWeight;
    float valueWeight;

    void Start()
    {       
        AddNumbersHeight();
        AddNumbersWeight();
    }

    void Update()
    {
        
    }

    //Height scroller methods
    public void AddNumbersHeight()
    {
        for (int i = 100; i <= 230; i++) {
            GameObject spawnedNumber = Instantiate(numberPrefabHeight, new Vector3(0f, 0f, 0f), Quaternion.identity, numberParentHeight);
            spawnedNumber.GetComponent<TextMeshProUGUI>().text =  i.ToString();//'\n' +
        }
    }

    public void GetValueHeight()
    {
        valueHeight = (float)Math.Round(numberParentHeight.localPosition.y / 100) + 100 ;
        Debug.Log("Height Value: " + valueHeight);
    }

    public string ReturnValueHeight()
    {
        return valueHeight.ToString();
    }

    //Weight scroller methods
    public void AddNumbersWeight()
    {
        for (int i = 40; i <= 200; i++)
        {
            GameObject spawnedNumber = Instantiate(numberPrefabWeight, new Vector3(0f, 0f, 0f), Quaternion.identity, numberParentWeight);
            spawnedNumber.GetComponent<TextMeshProUGUI>().text = i.ToString();//'\n' +
        }
    }

    public void GetValueWeight()
    {
        valueWeight = (float)Math.Round(numberParentWeight.localPosition.y / 100) + 40;
        Debug.Log("Weight Value: " + valueWeight);
    }

    public string ReturnValueWeight()
    {
        return valueWeight.ToString();
    }
}
