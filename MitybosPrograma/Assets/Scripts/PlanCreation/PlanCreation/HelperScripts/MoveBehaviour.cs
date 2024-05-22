using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using ZstdSharp.Unsafe;

public class MoveBehaviour : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	PlanFieldBehaviour p { get; set; }
	Vector3 InitialPosition { get; set; }
	Vector3 CurrentPosition { get; set; }
	GameObject parent { get; set; }
	Transform transformParent { get; set; }

	TableManager tableManager { get; set; }
	public void OnBeginDrag(PointerEventData eventData)
	{
		InitialPosition = transform.parent.position;
		transformParent = transform.parent;

		//need visuals
		Debug.Log("startedat: " + InitialPosition);
	}

	public void OnDrag(PointerEventData eventData)
	{
		CurrentPosition = transform.position;
		//Debug.Log("dragging, pos:" + CurrentPosition);
		transform.position = Input.mousePosition;
		
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		
		TableCell tc = 
			GetClosestTableCell(CurrentPosition,
			tableManager.CellList);
		Debug.Log("stopped" + transform.position+"closest cell " +tc.transform.position);
		transform.SetParent(null);
		
		tableManager.SetPrefabParent(transform.gameObject, tc.transform);
	}


	void Start()
	{
		Debug.Log(transform.parent.position);
		Debug.Log(transform.parent.parent.position);
		parent = transform.parent.gameObject;
		p = GetComponent<PlanFieldBehaviour>();
		tableManager = parent.GetComponentInParent<TableRow>().GetComponentInParent<TableManager>();
		if (tableManager != null )
		{
			Debug.Log("plan field script found");
			Debug.Log(tableManager.CellList.Count);
		}

	}

	// Update is called once per frame
	void Update()
	{

	}

	public TableCell GetClosestTableCell(Vector3 releasePosition, List<TableCell> cellList)
	{

		TableCell closestCell = null;
		float shortestDistance = Mathf.Infinity;

		foreach (var cell in cellList)
		{
			float distance = Mathf.Abs(cell.transform.position.y - releasePosition.y);
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				closestCell = cell;
			}
		}
		Debug.Log("Closest cell position: " + closestCell.transform.position);
		return closestCell;
	}

	public bool CompareDistance(Vector3 _, Vector3 __) => 
		(Mathf.Abs(_.y)> Mathf.Abs(__.y) ) ? true : false;

}