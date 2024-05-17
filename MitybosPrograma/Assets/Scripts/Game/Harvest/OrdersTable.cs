using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrdersTable : MonoBehaviour
{
    public GameObject orderPrefab; // Prefab for the order UI element
    public Transform ordersParent; // Parent object where orders will be instantiated
    public float offset;

    public void UpdateOrderTable(List<Order> orders, List<Customer> customers)
    {
        // Clear existing order UI elements
        foreach (Transform child in ordersParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate UI elements for each order
        float offsetY = 0f; // Initial y-offset
        foreach (Order order in orders)
        {
            GameObject orderObject = Instantiate(orderPrefab, ordersParent);

            // Set vitamin text
            orderObject.transform.Find("VitaminName").GetComponent<TextMeshProUGUI>().text = order.vitamin;
            // Set customer portrait
            orderObject.transform.Find("Customer").GetComponent<Image>().sprite = GetCustomerPortrait(order.customerName, customers);

            // Set the position of the order UI element
            orderObject.transform.localPosition = new Vector3(0f, offsetY, 0f);

            // Increment the y-offset for the next UI element
            offsetY -= offset; // Adjust this value as needed for spacing between elements
        }
    }

    // Get customer portrait based on customer name
    private Sprite GetCustomerPortrait(string customerName, List<Customer> customers)
    {
        // Search for the customer with the given name in the list of customers
        Customer customer = customers.Find(c => c.name == customerName);
        if (customer != null)
        {
            // Return the customer's portrait
            return customer.portrait;
        }
        else
        {
            // Return null or a default portrait if the customer is not found
            return null;
        }
    }

}
