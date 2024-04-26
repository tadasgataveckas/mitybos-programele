using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public SpriteRenderer sprite;
    public string itemName;
    private bool hovered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<VitaminHarvestPlayerManager>().nearbyPickableObjects.Add(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ChangeHover(false);
            other.gameObject.GetComponent<VitaminHarvestPlayerManager>().nearbyPickableObjects.Remove(gameObject);
        }
    }
    public void ChangeHover(bool isHover)
    {
        hovered = isHover;
        if(hovered)
        {
            sprite.color = Color.green;
        }
        else
        {
            sprite.color = Color.white;
        }
    }
}
