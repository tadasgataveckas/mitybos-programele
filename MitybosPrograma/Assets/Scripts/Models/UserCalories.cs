using static UserData;
using UnityEngine;
using System.Data;
using UnityEngine.Analytics;
using Mono.Data.Sqlite;

public class UserCalories
{
    public int id_user;
    public float bmi;
    public float calories;
    public string result;


    public UserCalories(int id)
    {
        id_user = id;
        bmi = 0;
        calories = 0;
        result = "You haven't synched yet, dumbass";
    }

    /// <summary>
    /// Updates UserCalories object according to id_user
    /// </summary>
    public void SynchData()
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_synch = DBManager.connection.CreateCommand();
            command_synch.CommandText =
                "SELECT bmi, calories, result FROM v_bmi_calculations " +
                "WHERE id_user = " + id_user;
            IDataReader reader = command_synch.ExecuteReader();

            if (reader.Read())
            {
                bmi = float.Parse(reader[0].ToString());
                calories = float.Parse(reader[1].ToString());
                result = reader[2].ToString();
            }
        }
        catch (SqliteException e) { System.Console.WriteLine(e.Message); }
        finally { DBManager.CloseConnection(); }
    }
}
