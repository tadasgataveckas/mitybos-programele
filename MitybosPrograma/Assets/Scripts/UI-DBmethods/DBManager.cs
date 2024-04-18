using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public static class DBManager
{
    private static TextAsset dbTables = Resources.Load<TextAsset>("food_db");
    private static TextAsset dbData = Resources.Load<TextAsset>("food_data");

    // database uri (required)
    private static string dbURI = "URI=file:";
    // database name
    private static string dbName = "food_db.db";
    private static string dbPath = Path.Combine(Application.persistentDataPath, dbName);

    // database connection
    public static IDbConnection connection;

    // creates base database
    public static void CreateDatabase()
    {
        //File.Create(dbPath);
        if (File.Exists(dbPath))
        {
            Debug.Log("Database already exists");
            return;
        }

        // Open connection
        OpenConnection();

        // create tables and data
        CreateTables();
        InsertData();

        // Close connection
        CloseConnection();
    }

    // creates base database
    public static void DeleteDatabase()
    {
        if (File.Exists(dbPath))
        {
            File.Delete(dbPath);
            Debug.Log("Database found and deleted");
        }
    }

    // creates base database tables
    private static void CreateTables()
    {
        IDbCommand command = connection.CreateCommand();
        command.CommandText = dbTables.text;
        command.ExecuteNonQuery();
    }

    // inserts base database data
    private static void InsertData()
    {
        IDbCommand command = connection.CreateCommand();
        command.CommandText = dbData.text;
        command.ExecuteNonQuery();
    }

    // reads all allergies
    private static void ReadDataTest()
    {
        OpenConnection();

        // Read and print all values in table
        IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM allergy";
        IDataReader reader = command.ExecuteReader();

        while (reader.Read() != false)
        {
            Debug.Log(reader[0].GetType().ToString());
        }

        CloseConnection();
    }

    // opens connection
    public static void OpenConnection()
    {
        // define connection to database
        if (connection == null)
            connection = new SqliteConnection(dbURI + dbPath);

        // if connection is not open
        if (connection.State != ConnectionState.Open)
            connection.Open();
    }

    // closes connection
    public static void CloseConnection()
    {
        if (connection.State != ConnectionState.Closed)
            connection.Close();
    }
}