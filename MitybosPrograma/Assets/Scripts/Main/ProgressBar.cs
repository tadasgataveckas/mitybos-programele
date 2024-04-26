using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public float max;
    public float curr;
    public Image mask;

    // Update is called once per frame
    //void Update()
    //{
    //    GetCurrFill();
    //}

    public void UpdateCurr()
    {
		GetCurrFill();
	}

    void GetCurrFill()
    {
        float fill_amount = curr / max;
        mask.fillAmount = fill_amount;
    }
}
