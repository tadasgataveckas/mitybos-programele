using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UsableObjectType
{
    FarmLand,
    Plant,
    Finish
}

public class UsableObject : MonoBehaviour
{
    public UsableObjectType type;
    private Plant plant;
    public SpriteRenderer sprite;
    public List<string> canBeUsedByTheseItems;
    private bool hovered = false;
    public bool disabled = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            string playerItem = other.gameObject.GetComponent<VitaminHarvestPlayerManager>().itemInHand;
            if (canBeUsedByTheseItems.Contains(playerItem))
            {
                other.gameObject.GetComponent<VitaminHarvestPlayerManager>().nearbyUsableObjects.Add(gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
             ChangeHover(false);
             other.gameObject.GetComponent<VitaminHarvestPlayerManager>().nearbyUsableObjects.Remove(gameObject);
        }
    }
    public void ChangeHover(bool isHover)
    {
        hovered = isHover;
        if (hovered)
        {
            sprite.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else
        {
            sprite.color = Color.white;
        }
    }

    void Update()
    {
        if (disabled && plant == null)
        {
            disabled = false;
        }
    }

    // Functionalities based on enum

    public void UseItem(string item)
    {
        ChangeHover(false);
        if (canBeUsedByTheseItems.Contains(item))
        {
            switch (type)
            {
                case UsableObjectType.FarmLand:
                    PlantSeed(item);
                    break;
                case UsableObjectType.Plant:
                    FinishGrowing();
                    break;
                case UsableObjectType.Finish:
                    Harvest();
                    break;
                default:
                    Debug.LogWarning("Unknown UsableObjectType: " + type);
                    break;
            }
        }
        else
        {
            Debug.LogWarning("This item cannot be used with " + item);
        }
    }

    private void PlantSeed(string item)
    {
        string foodName = item.Replace("Seed", "");
        RuntimeAnimatorController plantAnimator = VitaminHarvestItemManager.Instance.GetItemInformationByName(item).plantedSeedController;
        GameObject plantPrefab = VitaminHarvestItemManager.Instance.plantPrefab;
        GameObject spawnedPlant = Instantiate(plantPrefab, transform.position, Quaternion.identity, transform);
        plant = spawnedPlant.GetComponent<Plant>();
        spawnedPlant.GetComponent<PickableObject>().itemName = foodName;
        plant.StartGrowing(plantAnimator);
        disabled = true;
        Debug.Log("Planted seed on " + gameObject.name);
        // Implement planting logic here
    }

    private void FinishGrowing()
    {
        Debug.Log("Plant finished growing on " + gameObject.name);
        // Implement finish growing logic here
    }

    private void Harvest()
    {
        Debug.Log("Harvested on " + gameObject.name);
        // Implement harvesting logic here
    }
}
