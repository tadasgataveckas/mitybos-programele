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
    TMP_InputField InputField { get; set; }

	private List<TableCell> CellList = new List<TableCell>();
	[SerializeField]
	private GameObject Prefab;

    [SerializeField]
    private TableCell Cell0, Cell1, Cell2, Cell3, Cell4, Cell5, Cell6, Cell7, Cell8, Cell9;
	TableLayout _table { get; set; }
    private void Awake()
    {
		InputField = GameObject.Find("InputFieldNumber").GetComponent<TMP_InputField>();
		InputField.contentType = TMP_InputField.ContentType.Alphanumeric;
        _table = GetComponent<TableLayout>();
	}

	private void Start()
	{
		CellList.Add(Cell0);
		CellList.Add(Cell1);
		CellList.Add(Cell2);
		CellList.Add(Cell3);
		CellList.Add(Cell4);
		CellList.Add(Cell5);
		CellList.Add(Cell6);
		CellList.Add(Cell7);
		CellList.Add(Cell8);
		CellList.Add(Cell9);
	}

	private void Update()
    {
        OnSelectedDateUpdate();
    }
    public void OnClickAutoPlan()
    {
        string numberText = InputField.text;
        int number = Int32.Parse(numberText);

		ReturnedFoodList = c.ReturnFoodList();
        BranchAndBound branchAndBoundFood = new BranchAndBound(ReturnedFoodList);
        CreatedFoodList = branchAndBoundFood.FindClosestCalorieCombination(MainManager.userCalories.calories,number);

        Debug.Log(CreatedFoodList.Count + " vnt.");

        PlanFieldBehaviour p = new PlanFieldBehaviour();
        if(ReturnedFoodList.Count > 0)
        foreach (FoodClass food in CreatedFoodList)
        {
                foreach(var item in CellList)
                {
                    if(item.transform.childCount == 0)
                    {
						GameObject _prefab = Instantiate(Prefab);
                        
						p.SetPrefabParent(_prefab, item.transform);

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
