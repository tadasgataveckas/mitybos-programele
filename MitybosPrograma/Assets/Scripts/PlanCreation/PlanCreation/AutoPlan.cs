using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UI.Tables;
using System;

public class AutoPlan : MonoBehaviour
{
    ClientMethods c = new ClientMethods(new DatabaseMethods());
    List<FoodClass> ReturnedFoodList = new List<FoodClass>(); //allfoods
    List<FoodClass> CreatedFoodList = new List<FoodClass>(); //meals for the day
    public TextMeshProUGUI NumberText;

    //int index { get; set; }
    int[] numbers = { 1, 3, 5 };
    private int mealCount = 1;

    GameObject parent { get; set; }
    protected TableManager tableManager { get; set; }

    //change to +- buttons representing less mid and more

    TMP_InputField InputField { get; set; }


    [SerializeField]
    private GameObject Prefab;


    private void Start()
    {
        UpdateMealCounter();

        //switch (index)
        //{
        //    case 0:
        //        {
        //            plus1.enabled = true;
        //            plus3.enabled = false;
        //            plus5.enabled = false;
        //            break;
        //        }
        //    case 1:
        //        {
        //            plus1.enabled = true;
        //            plus3.enabled = true;
        //            plus5.enabled = false;
        //            break;
        //        }
        //    case 2:
        //        {
        //            plus1.enabled = true;
        //            plus3.enabled = true;
        //            plus5.enabled = true;
        //            break;
        //        }
        //}
        InputField = GameObject.Find("InputFieldNumber").GetComponent<TMP_InputField>();
        InputField.contentType = TMP_InputField.ContentType.Alphanumeric;
        InputField.text = "1";
        parent = transform.parent.gameObject;
        tableManager = parent.GetComponentInChildren<TableLayout>().GetComponentInChildren<TableManager>();
    }

    public void OnClickAutoPlan()
    {
        ReturnedFoodList = c.ReturnFoodList();
        BranchAndBound branchAndBoundFood = new BranchAndBound(ReturnedFoodList);
        CreatedFoodList = branchAndBoundFood.FindClosestCalorieCombination(MainManager.userCalories.calories, mealCount);

        foreach (FoodClass food in CreatedFoodList)
        {
            foreach (var item in tableManager.CellList)
            {
                if (item.transform.childCount == 0)
                {
                    GameObject _prefab = Instantiate(Prefab);

                    tableManager.SetPrefabParent(_prefab, item.transform);

                    SetInitPrefabInfo(_prefab, food.Name + " " + food.Calories + " kcal");

                    _prefab.SetActive(true);
                    break;
                }
                else
                {
                    //langelis uzimtas einam i kita
                }
            }
        }
    }

    public void OnPressGreater()
    {
        // there are 5 meal slots, but only up to 4 can be generated at once?
        if (mealCount < 4)
            mealCount++;
        UpdateMealCounter();
    }

    public void OnPressLess()
    {
        if (mealCount > 1)
            mealCount--;
        UpdateMealCounter();
    }

    private void SetInitPrefabInfo(GameObject prefab, string name)
    {
        TMP_Text prefabText = prefab.GetComponentInChildren<TMP_Text>();
        prefabText.text = name;
    }

    private void UpdateMealCounter()
    {
        NumberText.SetText(mealCount.ToString());
    }

}
