using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEditor;
using UI.Dates;
using UI.Tables;
using Org.BouncyCastle.Utilities;
using System;

public class AutoPlan : MonoBehaviour
{
    ClientMethods c = new ClientMethods(new DatabaseMethods());
    List<FoodClass> ReturnedFoodList = new List<FoodClass>(); //allfoods
    List<FoodClass> CreatedFoodList = new List<FoodClass>(); //meals for the day


    GameObject parent { get; set; }
    TableManager tableManager { get; set; }

    //change to +- buttons representing less mid and more
    TMP_InputField InputField { get; set; }


	[SerializeField]
	private GameObject Prefab;
    
    
    private void Start()
    {
        Debug.Log("+Button started, getting the table manager");
		InputField = GameObject.Find("InputFieldNumber").GetComponent<TMP_InputField>();
		InputField.contentType = TMP_InputField.ContentType.Alphanumeric;
		parent = transform.parent.gameObject;
		tableManager = parent.GetComponentInChildren<TableLayout>()?.GetComponentInChildren<TableManager>();
	}


	private void Update()
    {
        //OnSelectedDateUpdate();
    }
    public void OnClickAutoPlan()
    {
        string numberText = InputField.text;
        int number = Int32.Parse(numberText);

		ReturnedFoodList = c.ReturnFoodList();
        BranchAndBound branchAndBoundFood = new BranchAndBound(ReturnedFoodList);
        CreatedFoodList = branchAndBoundFood.FindClosestCalorieCombination(MainManager.userCalories.calories,number);

        Debug.Log(CreatedFoodList.Count + " vnt.");

        if(ReturnedFoodList.Count > 0)
        foreach (FoodClass food in CreatedFoodList)
        {
                foreach(var item in tableManager.CellList)
                {
                    if(item.transform.childCount == 0)
                    {
						GameObject _prefab = Instantiate(Prefab);
                        
						tableManager.SetPrefabParent(_prefab, item.transform);

						SetInitPrefabInfo(_prefab, food.Name);

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

    private void SetInitPrefabInfo(GameObject prefab, string name)
    {
        TMP_Text prefabText = prefab.GetComponentInChildren<TMP_Text>();
        prefabText.text = name;
    }

}
