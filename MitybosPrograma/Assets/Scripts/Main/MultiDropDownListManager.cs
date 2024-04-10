using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MultiDropDownListManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI textList;
    private List<int> allergies = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        UnselectDropdown();
    }

    public void InputAllergy(int val)
    {
        if (val == 9)
            return;
        UnselectDropdown();

        // ++, cuz dropdown values start at 0
        val = val + 1;

        if (allergies.Contains(val))
        {
            allergies.Remove(val);
            dropdown.options[val - 1].text = Allergy.ReturnAllergyName(val);
        }
        else
        {
            allergies.Add(val);
            dropdown.options[val - 1].text = "O - " + Allergy.ReturnAllergyName(val);
        }

        textList.text = "";
        foreach (int i in allergies)
            textList.text = textList.text + Allergy.ReturnAllergyName(i) + ", ";

        if (textList.text.Length > 0)
            textList.text = textList.text.Substring(0, textList.text.Length - 2);
    }

    // this is added cuz you can't select "nothing", and input allergy triggers
    // on value "changed" and not "selected"
    public void UnselectDropdown()
    {
        dropdown.options.Add(new TMP_Dropdown.OptionData());
        dropdown.value = 10;
        dropdown.options.RemoveAt(9);
    }
}
