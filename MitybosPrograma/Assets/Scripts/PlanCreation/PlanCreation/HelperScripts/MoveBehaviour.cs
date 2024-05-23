using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBehaviour : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	//Transform transformParent { get; set; }
	//Vector3 InitialPosition { get; set; }
	Vector3 CurrentPosition { get; set; }
	GameObject parent { get; set; }
	TableManager tableManager { get; set; }
	public void OnBeginDrag(PointerEventData eventData)
	{
		//InitialPosition = transform.parent.position;
		//transformParent = transform.parent;
		//need visualsxdf
		//Debug.Log("startedat: " + InitialPosition);
	}

	public void OnDrag(PointerEventData eventData)
	{
		CurrentPosition = transform.position;
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		
		TableCell tc = 
			GetClosestTableCell(CurrentPosition,
			tableManager.CellList);
		//Debug.Log("stopped" + transform.position+"closest cell " +tc.transform.position);
		if (tc.transform.childCount == 0)
		{
			transform.SetParent(null);
			tableManager.SetPrefabParent(transform.gameObject, tc.transform);
		}
		else
		{
			Transform parent = transform.parent;
			Transform otherPrefab=tc.transform.GetChild(0);
			transform.SetParent(null);
			tableManager.SetPrefabParent(transform.gameObject, tc.transform);
			otherPrefab.SetParent(null);
			tableManager.SetPrefabParent(otherPrefab.gameObject, parent);
		}
	}


	void Start()
	{
		//Debug.Log(transform.parent.position);
		//Debug.Log(transform.parent.parent.position);
		Transform parent = transform.parent;
		tableManager = parent.GetComponentInChildren<TableLayout>().GetComponentInChildren<TableManager>();

		//parent.transform.GetComponentInParent<TableRow>().GetComponentInParent<TableManager>();
	}

	private void Update()
	{
			
	}

	public TableCell GetClosestTableCell(Vector3 releasePosition, List<TableCell> cellList)
	{

		TableCell closestCell = null;
		float shortestDistance = Mathf.Infinity;

		foreach (var cell in cellList)
		{
			float distance = Mathf.Abs(cell.transform.position.y - releasePosition.y-130.0f); //offset for crispier dragging 
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				closestCell = cell;
			}
		}
		//Debug.Log("Closest cell position: " + closestCell.transform.position);
		return closestCell;
	}

	public bool CompareDistance(Vector3 _, Vector3 __) => 
		(Mathf.Abs(_.y)> Mathf.Abs(__.y) ) ? true : false;

}
