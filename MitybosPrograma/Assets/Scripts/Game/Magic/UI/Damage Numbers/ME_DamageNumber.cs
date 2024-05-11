using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ME_DamageNumber : MonoBehaviour
{
    public TextMeshProUGUI textField;
    [SerializeField] AnimationClip clip;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void DisplayDamage(float damage)
    {
        textField.text = Math.Round(damage, 1).ToString();
        Destroy(gameObject, clip.length);
    }
}
