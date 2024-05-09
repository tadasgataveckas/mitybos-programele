using Google.Protobuf.WellKnownTypes;
using Mysqlx.Crud;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Tables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class PlanFieldBehaviour : MonoBehaviour
{
    protected TMP_Text InfoText { get; set; }
    protected Button EditButton { get; set; }
    protected Button RemoveButton { get; set; }
    private GameObject parent { get; set; }
    public List<TableCell> cellList = new List<TableCell>();
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private TableCell Cell0, Cell1, Cell2, Cell3, Cell4, Cell5, Cell6, Cell7, Cell8, Cell9;
    void Start()
    {
		
		parent = transform.parent.gameObject;
		cellList.Add(Cell0);
		cellList.Add(Cell1);
		cellList.Add(Cell2);
		cellList.Add(Cell3);
		cellList.Add(Cell4);
		cellList.Add(Cell5);
		cellList.Add(Cell6);
		cellList.Add(Cell7);
		cellList.Add(Cell8);
		cellList.Add(Cell9);                                        
	}

    public void OnEditButtonClick()
    {
		InfoText = GetComponentInChildren<TMP_Text>();
		InfoText.text = "Editbutoun";
    }

    public void OnRemoveButtonClick()
    {
        Destroy(parent);
    }

    public void OnAddButtonClick()
    {
        foreach( var item in cellList )
        {
            if (item.transform.childCount == 0)
            {
				GameObject _prefab = Instantiate(prefab);
                SetPrefabParent(_prefab, item.transform);
                _prefab.transform.localPosition = Vector3.zero;
				
				EditButton = GameObject.Find("EditButton").GetComponent<Button>();
				
				RemoveButton = GameObject.Find("RemoveButton").GetComponent<Button>();
                
                break;
			}
        }
    }

    public void SetPrefabParent(GameObject o, Transform parent)
    {
        o.transform.SetParent(parent);
    }       
}
