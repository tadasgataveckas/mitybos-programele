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
	public List<TableCell> CellList { get; set; } = new List<TableCell>();

	[SerializeField]
	TableLayout tableLayout;
	private void Awake()
	{
		//Debug.Log("Table is rendered, setting the cell list in TableLayout");
		for (int i = 0; i < tableLayout.Rows.Count; i++)
		{
			TableRow row = tableLayout.Rows[i];
			CellList.Add(row.GetComponentInChildren<TableCell>());
		}
	}
	

	private void Update()
	{
		OnSelectedDateUpdate();	
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
	//add saving and loading
}
