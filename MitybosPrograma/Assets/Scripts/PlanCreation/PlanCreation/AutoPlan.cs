using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEditor;
using UI.Dates;

public class AutoPlan : MonoBehaviour
{
    ClientMethods c = new ClientMethods(new DatabaseMethods()); 
    List<FoodClass> returnedfoodlist = new List<FoodClass>(); //allfoods
    List<FoodClass> createdlist = new List<FoodClass>(); //meals for the day
    private void Awake()
    {
        //TMP_Text textobjectBreakfast = GameObject.Find("BreakfastFoodnameText").GetComponent<TMP_Text>();
        //TMP_Text textobjectLunch = GameObject.Find("LunchFoodnameText").GetComponent<TMP_Text>();
        //TMP_Text textobjectDinner = GameObject.Find("DinnerFoodnameText").GetComponent<TMP_Text>();
        //TMP_Text textobjectDate = GameObject.Find("SelectedDateText").GetComponent<TMP_Text>();

    }

    private void Update()
    {
        
        OnSelectedDateUpdate();
    }
    public void OnClickAutoPlan()
    {
        returnedfoodlist = c.ReturnFoodList();
        BranchAndBound branchAndBoundFood = new BranchAndBound(returnedfoodlist);
        createdlist = branchAndBoundFood.FindClosestCalorieCombination(MainManager.userCalories.calories,3);
        Debug.Log(createdlist.Count);
        TMP_Text textobjectBreakfast = GameObject.Find("BreakfastFoodnameText").GetComponent<TMP_Text>();
        TMP_Text textobjectLunch = GameObject.Find("LunchFoodnameText").GetComponent<TMP_Text>();
        TMP_Text textobjectDinner = GameObject.Find("DinnerFoodnameText").GetComponent<TMP_Text>();
        textobjectBreakfast.text = createdlist[0]?.ToString();
        textobjectLunch.text = createdlist[1]?.ToString();
        textobjectDinner.text = createdlist[2]?.ToString();
        
    }

    public void OnSelectedDateUpdate()
    {
        TMP_Text textobjectDate = GameObject.Find("SelectedDateText").GetComponent<TMP_Text>();
        DatePicker datePicker = GameObject.Find("DatePickerParent").GetComponent<DatePicker>();
        if (datePicker != null)
        {
            textobjectDate.text = "Plan for: " + datePicker.VisibleDate.ToString().Truncate(10,"");
        }
    }
}
