using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System;
using TMPro;
using System.Data;
using System.Collections.Generic;

public class ProductDetails
{
    public string Name;
    public float Kcal;
    public float Protein;
    public float Carbs;
    public float Fat;
}

public class FoodSearch : MonoBehaviour
{
   

    public TMP_InputField searchInputField;
    public ScrollRect scrollView;

    public GameObject resultPrefab;
    public GameObject productDetailPanel; 
    public GameObject searchPanel;
    public GameObject mainPanel;

    public TMP_Text productNameText; // Text component to display product name
    public TMP_Text kcalText; // Text component to display kcal
    public TMP_Text proteinText; // Text component to display protein
    public TMP_Text carbsText; // Text component to display carbs
    public TMP_Text fatText; // Text component to display fat

    public TMP_Text Total_kcalText; // Text component to display kcal
    public TMP_Text Total_proteinText; // Text component to display protein
    public TMP_Text Total_carbsText; // Text component to display carbs
    public TMP_Text Totoal_fatText; // Text component to display fat

    public TMP_InputField amountInputField;

    private MySqlConnection connection;
    private string connectionString;
    private ProductDetails selectedProduct;
    private List<ProductDetails> eatenProducts = new List<ProductDetails>(); // List to store eaten products

    void Start()
    {
        connectionString = "Server=localhost;User ID=root;Password=root;Database=food_db";
        connection = new MySqlConnection(connectionString);
        productDetailPanel.SetActive(false);
        searchPanel.SetActive(false);
        DisplayEatenProducts();
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

            if (!reader.HasRows)
            {
                Debug.Log("No such food :((");
                GameObject resultObj = Instantiate(resultPrefab, scrollView.content);
                resultObj.GetComponentInChildren<TMP_Text>().text = "No such food :((";
            }
            else
            {
                while (reader.Read())
                {
                    string name = reader.GetString("product_name");
                    float kcal = reader.GetFloat("kcal");
                    float protein = reader.GetFloat("protein");
                    float carbs = reader.GetFloat("carbohydrates");
                    float fat = reader.GetFloat("fat");

                    GameObject resultObj = Instantiate(resultPrefab, scrollView.content);

                    //resultObj.GetComponentInChildren<TMP_Text>().text = name + " - " + kcal + " kcal - Protein: " + protein + "g - Carbs: " + carbs + "g - Fat: " + fat + "g";
                    resultObj.GetComponentInChildren<TMP_Text>().text = name;
                    resultObj.GetComponent<Button>().onClick.AddListener(() => OnSearchResultClicked(name, kcal, protein, carbs, fat));
                }
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

    void OnSearchResultClicked(string productName, float kcal, float protein, float carbs, float fat)
    {
        // Populate selected product details
        selectedProduct = new ProductDetails
        {
            Name = productName,
            Kcal = kcal,
            Protein = protein,
            Carbs = carbs,
            Fat = fat
        };

        
        searchPanel.SetActive(false);

       
        productNameText.text = productName;
        kcalText.text = "Kcal: " + kcal.ToString();
        proteinText.text = "Protein: " + protein.ToString() + "g";
        carbsText.text = "Carbs: " + carbs.ToString() + "g";
        fatText.text = "Fat: " + fat.ToString() + "g";

        
        productDetailPanel.SetActive(true);
    }

    public void CloseProductDetail()
    {
        productDetailPanel.SetActive(false);
    }

    void ClearResultPanel()
    {
        foreach (Transform child in scrollView.content)
        {
            Destroy(child.gameObject);
        }
    }

    public void GoToMain()
    {
        mainPanel.SetActive(true);
        productDetailPanel.SetActive(false);
        searchPanel.SetActive(false);
        DisplayEatenProducts();
    }

    public void GoToSearch()
    {
        mainPanel.SetActive(false);
        productDetailPanel.SetActive(false);
        searchPanel.SetActive(true);
    }
    public void GoToDetailed()
    {
        mainPanel.SetActive(false);
        productDetailPanel.SetActive(true);
        searchPanel.SetActive(false);
    }


    public void OnAddToEatenList()
    {
        if (selectedProduct != null)
        {

            float amount = float.Parse(amountInputField.text);
            Debug.Log(amount);

            selectedProduct.Kcal = selectedProduct.Kcal * amount / 100;
            selectedProduct.Protein = selectedProduct.Protein * amount / 100;
            selectedProduct.Carbs = selectedProduct.Carbs * amount / 100;
            selectedProduct.Fat = selectedProduct.Fat * amount / 100;


            eatenProducts.Add(selectedProduct);
            Debug.Log(selectedProduct.Name + " added to eaten products list.");
            DisplayEatenProducts();
        }
        else
        {
            Debug.LogWarning("No product selected to add to eaten products list.");
        }
    }

    void DisplayEatenProducts()
    {
        float totalKcal = 0;
        float totalProtein = 0;
        float totalCarbs = 0;
        float totalFat = 0;

        foreach (ProductDetails product in eatenProducts)
        {
            totalKcal += product.Kcal;
            totalProtein += product.Protein;
            totalCarbs += product.Carbs;
            totalFat += product.Fat;
        }

        Total_kcalText.text = "Total Kcal: " + totalKcal.ToString();
        Total_proteinText.text = "Total Protein: " + totalProtein.ToString() + "g";
        Total_carbsText.text = "Total Carbs: " + totalCarbs.ToString() + "g";
        Totoal_fatText.text = "Total Fat: " + totalFat.ToString() + "g";
    }
}


