using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VitaminHarvestOrdersManager : MonoBehaviour
{
    // Timer
    public float TotalTime = 240f; // 4 minutes
    private float Timer; // current time left
    public TextMeshProUGUI TimeText; // Display time

    // Points and coins
    public int PointsCollected;
    public int CoinsCollected;
    public TextMeshProUGUI PointsText;
    public TextMeshProUGUI CoinsText;
    //
    // Order spawning info:
    public float TimeToPrepareOrder; // How much time do the customers wait before leaving
    // Points
    // 

    // If you prepare order, gain extra points, tips!! coins

    public List<Table> Tables;
    public List<Order> Orders;
    public OrdersTable ordersTable;
    public List<string> PossibleVitamins;
    public List<Customer> PossibleCustomers;
    public Transform CustomerSpawnPoint;
    public float RadiusFromCenter;
    public Transform DiningAreaCenter;

    public static VitaminHarvestOrdersManager Instance;

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

    void Start()
    {
        Timer = TotalTime;
        UpdateTimeDisplay();
        StartCoroutine(SpawnCustomersWithDelay(3, 1f));
    }

    // Coroutine to spawn customers with a delay
    private IEnumerator SpawnCustomersWithDelay(int numberOfCustomers, float delay)
    {
        for (int i = 0; i < numberOfCustomers; i++)
        {
            SpawnCustomer();
            yield return new WaitForSeconds(delay);
        }
    }

    void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer < 0)
            {
                Timer = 0;
            }
            UpdateTimeDisplay();
        }
        ordersTable.UpdateOrderTable(Orders, PossibleCustomers);
    }

    void UpdateTimeDisplay()
    {
        int minutes = Mathf.FloorToInt(Timer / 60);
        int seconds = Mathf.FloorToInt(Timer % 60);
        TimeText.text = $"{minutes:00}:{seconds:00}";
    }

    public void SpawnCustomer()
    {
        // Filter possible customers to find those who are not already spawned
        List<Customer> availableCustomers = PossibleCustomers.FindAll(c => c.spawnedObject == null);

        if (availableCustomers.Count > 0)
        {
            // Select a random customer from the available customers
            Customer selectedCustomer = availableCustomers[UnityEngine.Random.Range(0, availableCustomers.Count)];

            // Instantiate the customer's prefab at the spawn point
            Vector3 randomOffset1 = new Vector3(UnityEngine.Random.Range(-RadiusFromCenter, RadiusFromCenter), UnityEngine.Random.Range(-RadiusFromCenter, RadiusFromCenter), 0);
            GameObject spawnedObject = Instantiate(selectedCustomer.prefab, CustomerSpawnPoint.position + randomOffset1, Quaternion.identity);
            CustomerMovement customerComponent = spawnedObject.GetComponent<CustomerMovement>();

            // Assign the instantiated object to the customer's spawnedObject attribute
            selectedCustomer.spawnedObject = spawnedObject;

            // Sending to a random position around the dining area center
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-RadiusFromCenter, RadiusFromCenter), UnityEngine.Random.Range(-RadiusFromCenter, RadiusFromCenter), 0);
            // Move the customer to the dining area center
            customerComponent.SetTargetPosition(DiningAreaCenter.position + randomOffset, "DiningAreaCenter");

            // When the customer reaches the dining area center, move to an empty table
            StartCoroutine(MoveToTableAfterDelay(customerComponent, selectedCustomer));
        }
        else
        {
            Debug.LogWarning("No available customers to spawn.");
        }
    }

    private IEnumerator MoveToTableAfterDelay(CustomerMovement customerMovement, Customer selectedCustomer)
    {
        // Wait until the customer reaches the dining area center
        yield return new WaitUntil(() => !customerMovement.isMoving);

        // Wait for a random time between 0.5s and 1.5s
        float randomDelay = UnityEngine.Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(randomDelay);

        // Find all empty tables
        List<Table> emptyTables = Tables.FindAll(table => table.seatedCustomerName == "");

        if (emptyTables.Count > 0)
        {
            // Select a random empty table from the list
            Table emptyTable = emptyTables[UnityEngine.Random.Range(0, emptyTables.Count)];
            emptyTable.seatedCustomerName = selectedCustomer.name;

            Vector3 seatPosition = emptyTable.tableObject.GetComponent<CustomerTable>().SeatPosition.position + new Vector3(0f, -0.15f, 0f);
            // Move the customer to the empty table's position
            customerMovement.SetTargetPosition(seatPosition, "Table");

            // Wait until the customer reaches the table
            yield return new WaitUntil(() => !customerMovement.isMoving);

            customerMovement.SitAtTable(emptyTable.tableObject.transform.position);
            GenerateOrder(selectedCustomer.name, emptyTable.number);
        }
        else
        {
            Debug.LogWarning("No empty tables available.");
        }
    }
    public void GenerateOrder(string customerName, int tableNumber)
    {
        // Choose a random vitamin from the list of possible vitamins
        string randomVitamin = PossibleVitamins[UnityEngine.Random.Range(0, PossibleVitamins.Count)];

        // Set the time left to prepare the order
        float timeLeft = TimeToPrepareOrder;

        // Create a new order instance
        Order newOrder = new Order()
        {
            vitamin = randomVitamin,
            timeLeft = timeLeft,
            customerName = customerName,
            tableNumber = tableNumber
        };

        // Add the new order to your list of orders
        Orders.Add(newOrder);

        //Debug.Log($"New order generated: {randomVitamin} for {customerName} at table {tableNumber}. Time left: {timeLeft} seconds.");
    }
    public void FinishOrder(int tableNumber, string meal, GameObject spawnedMeal)
    {
        // Get the vitamin information from the meal
        string vitaminOfMeal = VitaminHarvestItemManager.Instance.GetItemInformationByName(meal).vitamin;

        // Find the order that matches the given table number
        Order orderToFinish = Orders.Find(order => order.tableNumber == tableNumber);
        Table tableToFinish = Tables.Find(table => table.number == tableNumber);


        // Check if the order exists and if the vitamin matches
        if (orderToFinish != null)
        {
            if (vitaminOfMeal == "")
            {
                VitaminHarvestItemManager.Instance.SpawnVFX("ReactionQuestion", tableToFinish.tableObject.transform.position);
            }
            else
            {
                if (orderToFinish.vitamin == vitaminOfMeal)
                {
                    VitaminHarvestItemManager.Instance.SpawnVFX("GrowEnd", tableToFinish.tableObject.transform.position);
                    VitaminHarvestItemManager.Instance.SpawnVFX("ReactionAwesome", tableToFinish.tableObject.transform.position);
                    PointsCollected += 10; // Example: Add points for completing the order
                    CoinsCollected += 5; // Example: Add coins for completing the order

                    PointsText.text = PointsCollected + " Points";
                    CoinsText.text = CoinsCollected + " g";
                }
                else
                {
                    VitaminHarvestItemManager.Instance.SpawnVFX("ReactionBad", tableToFinish.tableObject.transform.position);
                    Debug.LogWarning($"The vitamin of the meal ({vitaminOfMeal}) does not match the order vitamin ({orderToFinish.vitamin}).");
                }
                // Perform the necessary actions to finish the order
                Orders.Remove(orderToFinish);

                // Update the UI or perform other actions as necessary
                ordersTable.UpdateOrderTable(Orders, PossibleCustomers);

                // Eat meal
                Destroy(spawnedMeal);
                // Send the customer home
                StartCoroutine(SendCustomerHome(PossibleCustomers.Find(c => c.name == orderToFinish.customerName)));

                Debug.Log($"Order for table {tableNumber} finished successfully. Points and coins updated.");

            }
        }
        else
        {
            Debug.LogWarning($"No order found for table number {tableNumber}.");
        }
    }

    private IEnumerator SendCustomerHome(Customer selectedCustomer)
    {
        yield return new WaitForSeconds(0.5f);
        CustomerMovement customerMovement = selectedCustomer.spawnedObject.GetComponent<CustomerMovement>();
        customerMovement.StandUp();
        yield return new WaitForSeconds(0.5f);

        // Step 1: Move the customer to the dining area center
        customerMovement.SetTargetPosition(DiningAreaCenter.position, "DiningAreaCenter");

        // Wait until the customer reaches the dining area center
        yield return new WaitUntil(() => !customerMovement.isMoving);

        // Step 2: Wait for a short delay (optional)
        yield return new WaitForSeconds(0.5f);

        // Step 3: Move the customer to the spawn point
        customerMovement.SetTargetPosition(CustomerSpawnPoint.position, "SpawnPoint");

        // Wait until the customer reaches the spawn point
        yield return new WaitUntil(() => !customerMovement.isMoving);

        // Step 4: Find the table the customer was seated at and clear the seated customer name
        Table customerTable = Tables.Find(table => table.seatedCustomerName == selectedCustomer.name);
        if (customerTable != null)
        {
            customerTable.seatedCustomerName = "";
        }

        // Step 5: Destroy the customer object
        Destroy(selectedCustomer.spawnedObject);
        selectedCustomer.spawnedObject = null;

        // Update the customer list or perform any other necessary cleanup
        Debug.Log($"Customer {selectedCustomer.name} has been sent home and their table is now free.");

        // Step 6: Wait for a random time between 2 and 4 seconds
        float randomDelay = UnityEngine.Random.Range(2f, 4f);
        yield return new WaitForSeconds(randomDelay);

        // Step 7: Spawn a new customer
        SpawnCustomer();

    }
}
[Serializable]
public class Order
{
    public string vitamin;
    public float timeLeft;
    public string customerName;
    public int tableNumber;
    // Completed
}

[Serializable]
public class Customer
{
    public GameObject prefab;
    public string name; // To not spawn the customer who is already in restaurant
    public Sprite portrait; // To display in orders table
    public GameObject spawnedObject;
}

[Serializable]
public class Table
{
    public GameObject tableObject;
    public int number;
    public string seatedCustomerName;

}
