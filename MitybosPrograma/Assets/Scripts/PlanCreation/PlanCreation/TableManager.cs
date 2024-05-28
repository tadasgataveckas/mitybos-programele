using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Dates;
using UI.Tables;
using Unity.VisualScripting;
using UnityEngine;

public class TableManager : MonoBehaviour
{
	[SerializeField]
	TMP_Text dateText;
	[SerializeField]
	DatePicker datePicker;
	public List<TableCell> CellList { get; set; } = new List<TableCell> ();

	[SerializeField]
	TableLayout tableLayout;

	DateTime currentDate;

	[SerializeField]
	GameObject prefab;

	private Dictionary<DateTime, List<string>> tableData = new Dictionary<DateTime, List<string>>();
	private List<string> currentItems = new List<string>(new string[5]);



	private void Awake()
	{
		
		currentDate = DateTime.Now.Date;
		//SaveTableData(currentDate);
		Debug.Log("current date: " + currentDate);
		Debug.Log("Table is rendered, setting the cell list in TableLayout");
		for (int i = 0; i < tableLayout.Rows.Count; i++)
		{
			TableRow row = tableLayout.Rows[i];
			CellList.Add(row.GetComponentInChildren<TableCell>());
		}
	}
	

	private void Update()
	{
		if(datePicker.VisibleDate != currentDate)
		{
			OnSelectedDateUpdate();
			SaveTableData(currentDate);
			currentDate = DateTime.Parse(datePicker.VisibleDate.ToString()).Date;
			LoadTableData(currentDate);
			//Debug.Log("Date changed, current date: " + currentDate);
			//OnDateChanged(currentDate);
			Debug.Log("Performed write/read from dictionary");
		}
		
	}

	public void OnSelectedDateUpdate()
	{
		if (datePicker != null)
		{
			dateText.text = "Plan for: " + datePicker.VisibleDate.ToString().Truncate(10,"");
		}
	}

	public void SetPrefabParent(GameObject o, Transform parent)
	{
		o.transform.SetParent(parent);
	}

	public void OnDateChanged(DateTime newDate)
	{
		
		SaveTableData(currentDate);
		currentDate = newDate;
		LoadTableData(newDate);
	}

	private void SaveTableData(DateTime date)
	{
		WriteListItems(CellList);
		if (tableData.ContainsKey(date))
		{
			tableData[date] = new List<string>(currentItems);
			currentItems = new List<string>(new string[5]);
		}
		else
		{
			tableData.Add(date, new List<string>(currentItems));
			currentItems = new List<string>(new string[5]);
		}
	}

	private void LoadTableData(DateTime date)
	{
		
		foreach(TableCell item in CellList)
		{
			if(item.transform.childCount > 0)
			{
				Debug.Log("Destroying prefabs in table");
				Destroy(item.transform.GetChild(0).gameObject);
			}
		}
		if (tableData.ContainsKey(date))
		{
			currentItems = new List<string>(tableData[date]);
			ReadListItems(currentItems);
		}
		else
		{
			// No data for this date, clear the table or set default values
			currentItems = new List<string>(new string[5]);
			//ReadListItems(currentItems);
		}
	}

	private void WriteListItems(List<TableCell> items)
	{
		
		Transform prefab;
		Debug.Log("Items count in table:" + items.Count);
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].transform.childCount > 0)
			{
				Debug.Log("Child found in " + i +" getting the prefab");
				prefab = items[i].transform.GetChild(0);
				Debug.Log("Writing text to table data dictionary from " + prefab.name);
				currentItems[i] = prefab.GetComponentInChildren<TMP_Text>().text;
				Debug.Log("Write text to currentItems[] " + GameObject.Find("FoodInformation").GetComponent<TMP_Text>().text);
			}
			else
			{
				currentItems[i] = null;
			}
		}
		Debug.Log("currentItems after writing: " + string.Join(",", currentItems));

		//for (int i = 0;i<currentItems.Count;i++)
		//{
		//	if (currentItems[i] != null)
		//	Debug.Log(currentItems[i].ToString());
		//}
		
		//Debug.Log("Resetting the current items");
	}

	private void ReadListItems(List<string> items)
	{
		GameObject _prefab;
		for(int i = 0; i<items.Capacity; i++)
		{
			if (items[i] != null)
			{
				_prefab = Instantiate(prefab);
				TMP_Text t = _prefab.GetComponentInChildren<TMP_Text>();
				Debug.Log("returned text while reading: " + _prefab.GetComponentInChildren<TMP_Text>().GetComponent<TMP_Text>().text);
				t.text = items[i];
				SetPrefabParent(_prefab, CellList[i].transform);
			}
;
		}
	}


	//add saving and loading
}
