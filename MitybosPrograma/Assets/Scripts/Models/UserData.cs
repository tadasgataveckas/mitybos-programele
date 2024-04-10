using Mono.Data.Sqlite;
using System;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime;

public class UserData
{
    public int id_user;
    public double weight;
    public double height;
    public Gender gender;
    public Goal goal;
    public int physical_activity;
    public string date_of_birth;
    public string creation_date;

    public UserData(int id)
    {
        id_user = id;
        weight = 0;
        height = 0;
        gender = 0;
        goal = 0;
        physical_activity = 0;
        date_of_birth = "";
        creation_date = "";
    }

    public enum Gender
    {
        Null = 0,
        Male = 1,
        Female = 2,
        Other = 3
    }
    public enum Goal
    {
        Null = 0,
        LoseWeight = 1,
        MaintainWeight = 2,
        GainWeight = 3
        //    ,
        //GainMuscle = 4
    }

    /// <summary>
    /// Returns goal value as string
    /// </summary>
    /// <returns>Goal string value</returns>
    public string GetGoalString()
    {
        switch (goal)
        {
            case Goal.LoseWeight:
                return "Lose weight";
            case Goal.MaintainWeight:
                return "Maintain weight";
            case Goal.GainWeight:
                return "Gain weight";
            //case Goal.GainMuscle:
            //    return "Gain muscle";
            default:
                return "";
        }
    }

    /// <summary>
    /// Returns gender value as string
    /// </summary>
    /// <returns>Gender string</returns>
    public string GetGenderString()
    {
        return gender.ToString();
    }

    /// <summary>
    /// Returns Goal enum equivalent of string
    /// </summary>
    /// <param name="goal_string"></param>
    /// <returns>Goal enum</returns>
    public static Goal ParseGoal(string goal_string)
    {
        switch (goal_string)
        {
            case "Lose weight":
                return UserData.Goal.LoseWeight;
            case "Maintain weight":
                return UserData.Goal.MaintainWeight;
            case "Gain weight":
                return UserData.Goal.GainWeight;
            //case "Gain muscle":
            //    return UserData.Goal.GainMuscle;
            default:
                return UserData.Goal.Null;
        }
    }

    /// <summary>
    /// Returns Gender enum equivalent of string
    /// </summary>
    /// <param name="gender_string"></param>
    /// <returns>Gender enum</returns>
    public static Gender ParseGender(string gender_string)
    {
        return (Gender)Enum.Parse(typeof(UserData.Gender), gender_string);
    }

    /// <summary>
    /// Updates UserData object according to id_user
    /// </summary>
    public void SynchData()
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_synch = DBManager.connection.CreateCommand();
            command_synch.CommandText =
                "SELECT weight, height, gender, goal, physical_activity, " +
                "date_of_birth, creation_date " +
                "FROM user_data " +
                "WHERE id_user = " + id_user;
            IDataReader reader = command_synch.ExecuteReader();

            if (reader.Read())
            {
                weight = double.Parse(reader[0].ToString());
                height = double.Parse(reader[1].ToString());
                gender = ParseGender(reader[2].ToString());
                goal = ParseGoal(reader[3].ToString());
                physical_activity = int.Parse(reader[4].ToString());
                date_of_birth = reader[5].ToString();

                creation_date = reader[6].ToString(); ;
            }
        }
        catch (SqliteException e) { System.Console.WriteLine(e.Message); }
        finally { DBManager.CloseConnection(); }
    }

    public override string ToString()
    {
        string lines =
            "id_user = " + id_user + "\n" +
            "weight = " + weight + "\n" +
            "height = " + height + "\n" +
            "gender = " + gender + "\n" +
            "goal = " + goal + "\n" +
            "physical_activity = " + physical_activity + "\n" +
            "date_of_birth = " + date_of_birth + "\n" +
            "creation_date = " + creation_date; 
        return lines;
    }
}
