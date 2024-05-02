using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    public Animator Hands;

    // If no item is in hand - player can PICK UP item
    // If an item is in hand - player can DROP or USE item 

    void Update()
    {
        itemText.text = "Item: " + itemInHand;
        pickUpButton.SetActive(itemInHand == "");
        useButton.SetActive(itemInHand != "");
        dropButton.SetActive(itemInHand != "");
        // Pick Up
        if (itemInHand == "")
        {
            FindNearestPickableObject();
            if (nearestPickableObject != null)
            {
                pickUpButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                pickUpButton.GetComponent<Button>().interactable = false;
            }
        }
        // Use or Drop
        else
        {
            FindNearestUsableObject();
            if (nearestUsableObject != null)
            {
                useButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                useButton.GetComponent<Button>().interactable = false;
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
            nearestPickableObject.GetComponent<PickableObject>().PickUp();
            nearbyPickableObjects.Remove(nearestPickableObject);
            pickUpButton.SetActive(false);
        }
        EndOfAction();
    }
    public void Use()
    {
        if (nearestUsableObject != null)
        {
            Debug.Log("Used on: " + nearestUsableObject.name);
            // Perform use logic here
            nearestUsableObject.GetComponent<UsableObject>().UseItem(itemInHand);
            nearbyUsableObjects.Remove(nearestUsableObject);
            useButton.SetActive(false);
            itemInHand = "";
        }
        EndOfAction();
    }
    public void Drop()
    {
        if (itemInHand != "")
        {
            Debug.Log("Dropped: " + itemInHand);
            // Perform drop logic here
            GameObject prefabToDrop = VitaminHarvestItemManager.Instance.GetItemInformationByName(itemInHand).droppedItemPrefab;

            // Check if the prefab is valid
            if (prefabToDrop != null)
            {
                Vector3 objectPosition = transform.position;
                Vector3 dropPosition = objectPosition - new Vector3(0f, 0.25f, 0f); //  drop it directly below
                GameObject droppedItem = Instantiate(prefabToDrop, dropPosition, Quaternion.identity);
                dropButton.SetActive(false);
                itemInHand = "";
                droppedItem.GetComponent<PickableObject>().Drop();
            }
            else
            {
                Debug.LogWarning("Prefab to drop is null!");
            }
        }
        EndOfAction();
    }

    public void UnequipItem()
    {
        dropButton.SetActive(false);
        itemInHand = "";
        EndOfAction();
    }


    private void EndOfAction()
    {
        Hands.runtimeAnimatorController = VitaminHarvestItemManager.Instance.GetItemInformationByName(itemInHand).handController;
    }

    // Calculations, finding nearest object for highlighting them, knowing which one to use

    void FindNearestPickableObject()
    {
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;
        nearestPickableObject = null;

        //Find the nearest one
        foreach (GameObject obj in nearbyPickableObjects)
        {
            float distance = Vector3.Distance(obj.transform.position, playerPosition);
            if (distance < minDistance && !obj.GetComponent<PickableObject>().disabled)
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
        nearestUsableObject = null;

        //Find the nearest one
        foreach (GameObject obj in nearbyUsableObjects)
        {
            float distance = Vector3.Distance(obj.transform.position, playerPosition);
            if (distance < minDistance && !obj.GetComponent<UsableObject>().disabled)
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
