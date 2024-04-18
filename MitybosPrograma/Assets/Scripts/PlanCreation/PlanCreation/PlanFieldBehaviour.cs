using Google.Protobuf.WellKnownTypes;
using Mysqlx.Crud;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlanFieldBehaviour : MonoBehaviour
{
    protected TMP_Text InfoText { get; set; }
    protected Button EditButton { get; set; }
    protected Button RemoveButton { get; set; }
    private GameObject parent { get; set; }
    [SerializeField]
    private GameObject prefab;
    void Start()
    {
        InfoText = GetComponentInChildren<TMP_Text>();
        EditButton = GameObject.Find("EditButton").GetComponent<Button>();
        RemoveButton = GameObject.Find("RemoveButton").GetComponent<Button>();
        parent = transform.parent.gameObject;
    }

    public void OnEditButtonClick()
    {
        InfoText.text = "Editbutoun";
    }

    public void OnRemoveButtonClick()
    {
        Destroy(parent);
    }

    public void OnAddButtonClick()
    {
        GameObject _prefab = Instantiate(prefab);
        SetPrefabParent(_prefab);
        _prefab.transform.position = new Vector3(240, 360);

        
    }

    private void SetPrefabParent(GameObject o)
    {
        GameObject container = GameObject.Find("FoodObjectContainer");  //.GetComponent<GameObject>();

        o.transform.SetParent(container.transform);
    }       
}
