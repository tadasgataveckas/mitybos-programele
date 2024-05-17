using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTable : MonoBehaviour
{
    public Transform SeatPosition;
    public int TableNumber;

    public void FinishOrder(string item, GameObject spawnedItem)
    {
        VitaminHarvestOrdersManager.Instance.FinishOrder(TableNumber, item, spawnedItem);
    }
}
