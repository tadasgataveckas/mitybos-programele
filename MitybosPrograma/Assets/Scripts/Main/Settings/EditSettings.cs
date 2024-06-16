using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Timeline.TimelineAsset;

public class EditSettings : MonoBehaviour
{
    [SerializeField] private ScrollerPrefab height;
    [SerializeField] private ScrollerPrefab weight;
    [SerializeField] private TMP_Dropdown gender;
    [SerializeField] private TMP_Dropdown goal;
    [SerializeField] private TMP_Dropdown activity;
    [SerializeField] private ScrollerPrefab date;
    [SerializeField] private TMP_Dropdown allergies;
    [SerializeField] private TMP_Dropdown preference;
    [SerializeField] private TextMeshProUGUI allergySettingDisplay;


    private UserData userData;
    private int id_user;
    private List<int> newAllergies;
    ClientMethods clientMethods;

    // Start is called before the first frame update
    void Awake()
    {
        clientMethods = new ClientMethods(new DatabaseMethods());
        id_user = SessionManager.GetIdKey();
        userData = new UserData(id_user);
        date.endIndex = DateTime.Now.Year;
        date.RefreshScroller();
    }

    private void OnEnable()
    {
        userData.SynchData();

        height.SetDefaultValue((int)userData.height);
        weight.SetDefaultValue((int)userData.weight);
        
        gender.value = (int)userData.gender - 1;
        goal.value = (int)userData.goal - 1;
        activity.value = (int)userData.physical_activity - 1;
        newAllergies = clientMethods.GetAllUserAllergies(id_user);

        if (newAllergies.Contains(10))
            preference.value = 1;
        else if (newAllergies.Contains(11))
            preference.value = 2;
        else
            preference.value = 0;

        foreach (int allergy in newAllergies)
            if (allergy < 10)
                allergies.options[allergy - 1].text = "O - " + Allergy.ReturnAllergyName(allergy);

        UnselectDropdown();
        UpdateAllergyDisplay();
    }

    public void InputHeight()
    {
        if (height != null)
        {
            float currHeight = height.GetValue();
            userData.height = currHeight;
        }
    }

    public void InputWeight()
    {
        if (weight != null)
        {
            float currWeight = weight.GetValue();
            userData.weight = currWeight;
        }
    }

    public void InputGender(int val)
    {
        userData.gender = (UserData.Gender)(val + 1);
    }

    public void InputGoal(int val1)
    {
        userData.goal = (UserData.Goal)(val1 + 1);
    }

    public void InputActivity(int val2)
    {
        userData.physical_activity = (val2 + 1);
    }

    public void InputYear()
    {
        if (date != null)
        {
            float currYear = date.GetValue();

            userData.date_of_birth = currYear.ToString();
        }
    }

    public void InputAllergy(int val)
    {
        // added to escape self call (9 is non existent item acting as null)
        if (val >= 9)
            return;

        UnselectDropdown();

        // ++, cuz dropdown values start at 0
        val = val + 1;

        if (newAllergies.Contains(val))
        {
            newAllergies.Remove(val);
            allergies.options[val - 1].text = Allergy.ReturnAllergyName(val);
        }
        else
        {
            newAllergies.Add(val);
            allergies.options[val - 1].text = "O - " + Allergy.ReturnAllergyName(val);
        }

        UpdateAllergyDisplay();
    }

    public void InputPreference(int val)
    {
        switch (val)
        {
            case 1:
                newAllergies.Remove(11);
                newAllergies.Add(10);
                break;
            case 2:
                newAllergies.Remove(10);
                newAllergies.Add(11);
                break;
            default:
                newAllergies.Remove(10);
                newAllergies.Remove(11);
                break;
        }
    }

    // this is added cuz you can't select "nothing", and input allergy triggers
    // on value "changed" and not "selected"
    private void UnselectDropdown()
    {
        allergies.options.Add(new TMP_Dropdown.OptionData());
        allergies.value = 10;
        allergies.options.RemoveAt(9);
    }

    private void UpdateAllergyDisplay()
    {
        allergySettingDisplay.text = "";
        foreach (int i in newAllergies)
            if (i < 10)
                allergySettingDisplay.text = allergySettingDisplay.text + Allergy.ReturnAllergyName(i) + ", ";

        if (allergySettingDisplay.text.Length > 0)
            allergySettingDisplay.text = allergySettingDisplay.text.Substring(0, allergySettingDisplay.text.Length - 2);
    }

    public void SubmitChanges()
    {
        clientMethods.DeleteUserAllergies(userData.id_user);
        foreach (int allergy in newAllergies)
            clientMethods.InsertUserAllergy(userData.id_user, allergy);

        clientMethods.UpdateUserData(userData);
        gameObject.SetActive(false);
    }
}
