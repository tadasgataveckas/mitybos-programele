using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminHarvestItemManager : MonoBehaviour
{
    public List<ItemInformation> itemsInformation = new List<ItemInformation>();
    public static VitaminHarvestItemManager Instance; // Singleton instance
    public GameObject plantPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of VitaminHarvestItemManager detected!");
            Destroy(gameObject);
        }
    }

    // Class to hold information about each item
    [Serializable]
    public class ItemInformation
    {
        public GameObject droppedItemPrefab;
        public string name;
        public RuntimeAnimatorController handController;
        public RuntimeAnimatorController plantedSeedController; // Seeds have this
    }

    public ItemInformation GetItemInformationByName(string itemName)
    {
        foreach (ItemInformation itemInfo in itemsInformation)
        {
            if (itemInfo.name == itemName)
            {
                return itemInfo;
            }
        }
        return null; // If item with the specified name is not found
    }

}
