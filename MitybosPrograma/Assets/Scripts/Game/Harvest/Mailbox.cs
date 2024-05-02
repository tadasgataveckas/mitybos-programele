using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailbox : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject Orders;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            sprite.color = Color.red;
            Orders.SetActive(true); // Veliau pakeisti
            // Paleisti isplesta uzsakymu informacija
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            sprite.color = Color.white;
            Orders.SetActive(false);
        }
    }
}
