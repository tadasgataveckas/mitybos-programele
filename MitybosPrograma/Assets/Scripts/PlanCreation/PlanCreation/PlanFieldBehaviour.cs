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

    TableManager tableManager { get; set; }

    [SerializeField]
    private GameObject prefab;

    void Start()
    {
        
		parent = transform.parent.gameObject;
        tableManager = parent.GetComponentInChildren<TableLayout>()?.GetComponentInChildren<TableManager>();
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
        foreach( var item in tableManager.CellList )
        {
            if (item.transform.childCount == 0)
            {
				GameObject _prefab = Instantiate(prefab);
                tableManager.SetPrefabParent(_prefab, item.transform);
                _prefab.transform.localPosition = Vector3.zero;
				
                //change to getting buttons in order
				EditButton = GameObject.Find("EditButton").GetComponent<Button>();
				RemoveButton = GameObject.Find("RemoveButton").GetComponent<Button>();
                
                break;
			}
        }
    }


}
