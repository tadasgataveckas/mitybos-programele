using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System;
using TMPro;
using System.Data;
using System.Collections.Generic;

//todo: save search results localy
//todo: result buttons should take to add menu

public class FoodSearch : MonoBehaviour
{
    //Unity fields
    public TMP_InputField searchInputField;
    public ScrollRect scrollView; 
    public GameObject resultPrefab;

    //DB conection
    private MySqlConnection connection;
    private string connectionString;

    void Start()
    {
        connectionString = "Server=localhost;User ID=root;Password=root;Database=food_db";
        connection = new MySqlConnection(connectionString);
    }

    public void Search()
    {
        string input = searchInputField.text;

        string query = "SELECT * FROM product WHERE product_name LIKE '%" + input + "%';";

        try
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            ClearResultPanel();

            while (reader.Read())
            {
                string name = reader.GetString("product_name");
                float kcal = reader.GetFloat("kcal");
                float protein = reader.GetFloat("protein");
                float carbs = reader.GetFloat("carbohydrates");
                float fat = reader.GetFloat("fat");

                GameObject resultObj = Instantiate(resultPrefab, scrollView.content);

                resultObj.GetComponentInChildren<TMP_Text>().text = name + " - " + kcal + " kcal - Protein: " + protein + "g - Carbs: " + carbs + "g - Fat: " + fat + "g";

                resultObj.GetComponent<Button>().onClick.AddListener(() => OnSearchResultClicked(name));
            }

            reader.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Error searching for food products: " + e.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }

    // Method to handle click event on search result
    void OnSearchResultClicked(string productName)
    {
        Debug.Log("Clicked on: " + productName);
    }

    // Method to clear the result panel
    void ClearResultPanel()
    {
        foreach (Transform child in scrollView.content)
        {
            Destroy(child.gameObject);
        }
    }
}