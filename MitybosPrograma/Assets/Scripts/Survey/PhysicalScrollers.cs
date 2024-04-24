using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PhysicalScrollers : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject[] objects; // Čia įrašykite objektus, kuriuos norite scroll'inti
    private RectTransform contentRect;
    public Transform numberParent;
    public GameObject numberPrefab;


    void Start()
    {
        
        AddNumbers();
    }

    void Update()
    {
        
        //GetValue();
    }

    
    public void AddNumbers()
    {
        for (int i = 100; i <= 230; i++) {
            GameObject spawnedNumber = Instantiate(numberPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, numberParent);
            spawnedNumber.GetComponent<TextMeshProUGUI>().text =  i.ToString();//'\n' +
        }
    }

    public void GetValue()
    {
        float value = (float)Math.Round(numberParent.localPosition.y / 100) + 100 ;
        Debug.Log("Value: " + value);
    }
}
