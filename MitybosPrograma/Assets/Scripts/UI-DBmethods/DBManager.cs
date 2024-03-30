using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public static class DBManager
{
    // path for all database files
    private static string dbPath = "Assets/Scripts/UI-DBmethods/";
    private static string dbTables = "food_db.txt";
    private static string dbData = "food_data.txt";

    // database uri
    private static string dbURI = "URI=file:";
    // database name
    private static string dbName = "food_db.db";

    // database connection
    public static IDbConnection connection;

    // creates base database
    public static void CreateDatabase()
    {
        if (File.Exists(dbPath + dbName))
            return;

        // Open connection
        OpenConnection();

        // create tables and data
        CreateTables();
        InsertData();

        // Close connection
        CloseConnection();
    }

    // creates base database tables
    private static void CreateTables()
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
    private static void InsertData()
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
            connection = new SqliteConnection(dbURI + dbPath + dbName);

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