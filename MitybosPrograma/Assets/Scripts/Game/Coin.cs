using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int Value = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            JumpGameManager jumpGameManager = JumpGameManager.Instance;
            jumpGameManager.coins += Value;
            Destroy(gameObject);

        }
    }
}
