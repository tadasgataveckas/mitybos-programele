using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminHarvestItemManager : MonoBehaviour
{
    public List<ItemInformation> itemsInformation = new List<ItemInformation>();
    public static VitaminHarvestItemManager Instance; // Singleton instance
    public GameObject plantPrefab;
    public List<VFXAssets> vfxAssets = new List<VFXAssets>();

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
        public string vitamin;
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

    [Serializable]
    public class VFXAssets
    {
        public string name;
        public GameObject vfxPrefab;
    }

    public void SpawnVFX(string name, Vector3 location)
    {
        foreach (VFXAssets vfx in vfxAssets)
        {
            if (vfx.name == name)
            {
                GameObject spawnedVFX = Instantiate(vfx.vfxPrefab, location, Quaternion.identity);
                // Start coroutine to destroy the spawned VFX after 5 seconds
                StartCoroutine(DestroyAfterDelay(spawnedVFX, 5f));
                return;
            }
        }
    }
    // Coroutine to destroy the object after a delay
    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

}
