using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VitaminHarvestPlayerManager : MonoBehaviour
{
    // Pick Up

    public List<GameObject> nearbyPickableObjects;

    private GameObject nearestPickableObject;

    public GameObject pickUpButton;

    public TextMeshProUGUI itemText;

    // Use

    public List<GameObject> nearbyUsableObjects;

    private GameObject nearestUsableObject;

    public GameObject useButton;

    // Drop

    public GameObject dropButton;

    // Item In Hand
    // Gali buti seklos, laistytuvas, uzaugintas produktas, lekste ar lekste su maistu

    public string itemInHand = "";

    // If no item is in hand - player can PICK UP item
    // If an item is in hand - player can DROP or USE item 

    void Update()
    {
        itemText.text = "Item: " + itemInHand;
        // Pick Up
        if (itemInHand == "")
        {
            if (nearbyPickableObjects.Count > 0)
            {
                FindNearestPickableObject();
                pickUpButton.SetActive(true);
            }
            else
            {
                pickUpButton.SetActive(false);
            }
        }
        // Use or Drop
        else
        {
            if (nearbyUsableObjects.Count > 0)
            {
                FindNearestUsableObject();
                useButton.SetActive(true);
            }
            else
            {
                useButton.SetActive(false);
            }
        }
    }

    
    public void PickUp()
    {
        if (nearestPickableObject != null)
        {
            Debug.Log("Picked up: " + nearestPickableObject.name);
            itemInHand = nearestPickableObject.GetComponent<PickableObject>().itemName;
            // Perform pickup logic here
            nearestPickableObject.GetComponent<PickableObject>().ChangeHover(false);
            nearbyPickableObjects.Remove(nearestPickableObject);
            pickUpButton.SetActive(false);
        }
    }
    public void Use()
    {
        if (nearestUsableObject != null)
        {
            Debug.Log("Used on: " + nearestPickableObject.name);
            itemInHand = "";
            // Perform use logic here
            nearestUsableObject.GetComponent<UsableObject>().ChangeHover(false);
            nearbyUsableObjects.Remove(nearestUsableObject);
            useButton.SetActive(false);
        }
    }

    // Calculations, finding nearest object for highlighting them, knowing which one to use

    void FindNearestPickableObject()
    {
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        //Find the nearest one
        foreach (GameObject obj in nearbyPickableObjects)
        {
            float distance = Vector3.Distance(obj.transform.position, playerPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPickableObject = obj;
            }
        }
        //Send signals to change visuals
        foreach (GameObject obj in nearbyPickableObjects)
        {
            obj.GetComponent<PickableObject>().ChangeHover(obj == nearestPickableObject);
        }
    }

    void FindNearestUsableObject()
    {
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        //Find the nearest one
        foreach (GameObject obj in nearbyUsableObjects)
        {
            float distance = Vector3.Distance(obj.transform.position, playerPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestUsableObject = obj;
            }
        }
        //Send signals to change visuals
        foreach (GameObject obj in nearbyUsableObjects)
        {
            obj.GetComponent<UsableObject>().ChangeHover(obj == nearestUsableObject);
        }
    }
}
