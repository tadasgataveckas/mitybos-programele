using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator animator;
    public string itemName;
    private bool hovered = false;
    public bool destroyAfterPicking;
    public bool disabled = false;

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
            sprite.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else
        {
            sprite.color = Color.white;
        }
    }
    public void PickUp()
    {
        ChangeHover(false);
        if (destroyAfterPicking)
        {
            Destroy(gameObject);
        }
    }
    public void Drop()
    {
        animator.SetTrigger("drop");
    }
}
