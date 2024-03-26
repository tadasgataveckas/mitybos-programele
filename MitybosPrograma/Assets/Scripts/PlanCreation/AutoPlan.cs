using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEditor;

public class AutoPlan : MonoBehaviour
{
    ClientMethods c = new ClientMethods(new DatabaseMethods()); 
    List<FoodClass> returnedfoodlist = new List<FoodClass>(); //allfoods
    List<FoodClass> createdlist = new List<FoodClass>(); //meals for the day
    private void Awake()
    {
        TMP_Text textobjectBreakfast = GameObject.Find("BreakfastFoodnameText").GetComponent<TMP_Text>();
        TMP_Text textobjectLunch = GameObject.Find("LunchFoodnameText").GetComponent<TMP_Text>();
        TMP_Text textobjectDinner = GameObject.Find("DinnerFoodnameText").GetComponent<TMP_Text>();
        

    }


    string constring = "Server=localhost;User ID=root;Password=root;Database=food_db";
    public void OnClickAutoPlan()
    {
        returnedfoodlist = c.ReturnFoodList(constring);
        BranchAndBound branchAndBoundFood = new BranchAndBound(returnedfoodlist);
        createdlist = branchAndBoundFood.FindClosestCalorieCombination(3000,3);
        Debug.Log(createdlist.Count);
        TMP_Text textobjectBreakfast = GameObject.Find("BreakfastFoodnameText").GetComponent<TMP_Text>();
        TMP_Text textobjectLunch = GameObject.Find("LunchFoodnameText").GetComponent<TMP_Text>();
        TMP_Text textobjectDinner = GameObject.Find("DinnerFoodnameText").GetComponent<TMP_Text>();
        textobjectBreakfast.text = createdlist[0].ToString();
        textobjectLunch.text = createdlist[1].ToString();
        textobjectDinner.text = createdlist[2].ToString();
    }
}
