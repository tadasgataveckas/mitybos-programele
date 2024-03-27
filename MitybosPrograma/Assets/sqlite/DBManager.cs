using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class DBManager : MonoBehaviour
{
    // path for all database files
    private static string dbPath = "Assets/sqlite/";
    private string dbTables = "food_db.txt";
    private string dbData = "food_data.txt";

    // database uri
    private static string dbURI = "URI=file:";
    // database name
    private static string dbName = "food_db.db";

    // database connection
    public static IDbConnection connection;

    // Start is called before the first frame update
    void Start()
    {
        // check if .db file exists
        if (!File.Exists(dbPath + dbName))
        {
            CreateDatabase();
            Debug.Log("Database created!");
        }

        ReadData();
        Debug.Log("Finished!");
    }

    // creates base database
    private void CreateDatabase()
    {
        // Open connection
        openConnection();

        // create tables and data
        CreateTables();
        InsertData();

        // Close connection
        closeConnection();
    }

    // creates base database tables
    private void CreateTables()
    {
        using (StreamReader sr = new StreamReader(dbPath + dbTables))
        {
            IDbCommand command = connection.CreateCommand();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                // insert tables in db
                command.CommandText += line + "\n";
            }
            command.ExecuteNonQuery();
        }
    }

    // inserts base database data
    private void InsertData()
    {
        //Read the text from directly from the test.txt file
        using (StreamReader sr = new StreamReader(dbPath + dbData))
        {
            IDbCommand command = connection.CreateCommand();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                // Insert values in tables
                command.CommandText += line;
            }
            command.ExecuteNonQuery();
        }
    }

    // reads all allergies
    private void ReadData()
    {
        openConnection();

        // Read and print all values in table
        IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM allergy";
        IDataReader reader = command.ExecuteReader();

        while (reader.Read() != false)
        {
            Debug.Log("Id: " + reader[0] + "; Allergy: " + reader[1]);
        }

        closeConnection();
    }

    // opens connection
    public static void openConnection()
    {
        // define connection to database
        if (connection == null)
            connection = new SqliteConnection(dbURI + dbPath + dbName);

        // if connection is not open
        if (connection.State != ConnectionState.Open)
            connection.Open();
    }

    // closes connection
    public static void closeConnection()
    {
        if (connection.State != ConnectionState.Closed)
            connection.Close();
    }
}