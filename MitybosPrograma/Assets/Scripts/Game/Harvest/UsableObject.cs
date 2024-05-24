using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UsableObjectType
{
    FarmLand,
    Plant,
    Finish,
    PlacementArea,
    CustomerTable
}

public class UsableObject : MonoBehaviour
{
    public UsableObjectType type;
    private Plant plant;
    public SpriteRenderer sprite;
    public Animator animator;
    public List<string> canBeUsedByTheseItems;
    private bool hovered = false;
    public bool disabled = false;
    public bool hasHoverAnimation;

    private GameObject player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
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
            if (hasHoverAnimation)
            {
                animator.SetBool("hover", true);
            }
        }
        else
        {
            sprite.color = Color.white;
            if (hasHoverAnimation)
            {
                animator.SetBool("hover", false);
            }
        }
    }

    void Update()
    {
        if (disabled && plant == null && transform.childCount == 1) // plant checking for farmlands, children count for placeable area, 1 child is reserved for visuals
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
                case UsableObjectType.PlacementArea:
                    PlaceOnArea(item);
                    break;
                case UsableObjectType.CustomerTable:
                    PlaceOnArea(item);
                    StartCoroutine(FinishOrderAfterDelay(item));
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
        VitaminHarvestItemManager.Instance.SpawnVFX("GrowStart", transform.position);
        //Debug.Log("Planted seed on " + gameObject.name);
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

    private void PlaceOnArea(string item)
    {
        // Drop item on my location
        GameObject prefabToDrop = VitaminHarvestItemManager.Instance.GetItemInformationByName(item).droppedItemPrefab;

        if (prefabToDrop != null)
        {
            GameObject droppedItem = Instantiate(prefabToDrop, transform.position, Quaternion.identity, transform);
            droppedItem.GetComponent<PickableObject>().Drop();
            disabled = true;

            player.GetComponent<VitaminHarvestPlayerManager>().UnequipItem();

            // Start the coroutine to set the sorting order after 0.1 seconds
            StartCoroutine(SetSortingOrderAfterDelay(droppedItem, 0.01f));
        }
        else
        {
            Debug.LogWarning("Prefab to drop is null!");
        }
    }

    private IEnumerator SetSortingOrderAfterDelay(GameObject droppedItem, float delay)
    {
        float position = transform.parent.position.y;
        // Get the first child, which is visuals
        SpriteRenderer spriteRenderer = droppedItem.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.CeilToInt(position * 1000) * -1 + 5;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer not found on the first child of the dropped item.");
        }

        yield return new WaitForSeconds(delay);

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.CeilToInt(position * 1000) * -1 + 5;
        }


    }

    private IEnumerator FinishOrderAfterDelay(string item)
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Check if the placed item is still not null
        if (transform.childCount > 1) // first child is placement area visuals
        {
            FinishOrder(item, transform.GetChild(1).gameObject); //second child
        }
    }

    private void FinishOrder(string item, GameObject spawnedItem)
    {
        transform.parent.gameObject.GetComponent<CustomerTable>().FinishOrder(item, spawnedItem);

    }
}
