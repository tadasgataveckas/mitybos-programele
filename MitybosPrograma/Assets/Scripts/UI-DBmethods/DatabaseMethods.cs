using System;
using System.Data;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Mono.Data.Sqlite;

public class DatabaseMethods
{
    public int Login(string username, string password, out int id, string constring)
    {
        id = -1;
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_login = DBManager.connection.CreateCommand();
            command_login.CommandText = "SELECT * FROM user WHERE " +
                                    "username = @expr1 AND password = @expr2;";

            // username parameter
            IDbDataParameter p_username = command_login.CreateParameter();
            p_username.ParameterName = "@expr1";
            p_username.Value = username;
            command_login.Parameters.Add(p_username);

            // password parameter
            IDbDataParameter p_password = command_login.CreateParameter();
            p_password.ParameterName = "@expr2";
            p_password.Value = password;
            command_login.Parameters.Add(p_password);

            using (IDataReader reader = command_login.ExecuteReader())
            {
                if (reader.Read() != false)
                {
                    id = int.Parse(reader[0].ToString());
                    return id;
                }
                else
                    id = -1;
                return id;
            }

        }
        catch (SqliteException e)
        {
            System.Console.WriteLine(e.Message);
            return id;
        }
        catch (FormatException e)
        {
            System.Console.WriteLine("Bad format \n" + e.Message);
            return id;
        }
        finally
        {
            DBManager.CloseConnection();
        }
    }

    public bool Register(string email, string username, string password, string constring)
    {
        try
        {
            DBManager.OpenConnection();

            // adds user to db -------------------------------------------------
            IDbCommand command_register = DBManager.connection.CreateCommand();
            command_register.CommandText =
                "INSERT INTO user " +
                "(email, username, password) " +
                "VALUES (@expr1, @expr2, @expr3);";

            IDbDataParameter p_email = command_register.CreateParameter();
            p_email.ParameterName = "@expr1"; p_email.Value = email;
            command_register.Parameters.Add(p_email);

            IDbDataParameter p_username = command_register.CreateParameter();
            p_username.ParameterName = "@expr2"; p_username.Value = username;
            command_register.Parameters.Add(p_username);

            IDbDataParameter p_password = command_register.CreateParameter();
            p_password.ParameterName = "@expr3"; p_password.Value = password;
            command_register.Parameters.Add(p_password);

            int rowCount = command_register.ExecuteNonQuery();

            // adds a placeholder for user_data --------------------------------
            IDbCommand command_select = DBManager.connection.CreateCommand();
            command_select.CommandText =
                "SELECT id_user FROM user WHERE" +
                "username = @expr1 AND " +
                "password = @expr2;";

            command_select.Parameters.Add(p_username);
            command_select.Parameters.Add(p_password);

            int id = -1;
            using (IDataReader reader = command_select.ExecuteReader())
            {
                if (reader.Read() != false)
                {
                    id = int.Parse(reader[0].ToString());
                }
            }
            if (rowCount > 0)
            {
                InsertRegisterPlaceholder(id, constring);
                return true;
            }
            else
                return false;
        }
        catch (SqliteException e)
        {
            System.Console.WriteLine(e.Message); return false;
        }
        finally { DBManager.CloseConnection(); }
    }

    public void InsertRegisterPlaceholder(int id, string constring)
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_insert = DBManager.connection.CreateCommand();
            command_insert.CommandText =
                "INSERT INTO user_data" +
                "(id_user, height, weight, gender, goal, physical_activity, date_of_birth)" +
                " VALUES(@expr1, 0.0, 0.0, 'Male', 'Lose weight', 1, CURRENT_TIMESTAMP);";

            IDbDataParameter p_id = command_insert.CreateParameter();
            p_id.ParameterName = "@expr1"; p_id.Value = id;
            command_insert.Parameters.Add(p_id);

            command_insert.ExecuteNonQuery();
        }
        catch (SqliteException e)
        {
            System.Console.WriteLine(e.Message);
        }
        finally { DBManager.CloseConnection(); }
    }

    public void UpdateProfile(int id, string gender, double height, double weight, string goal, string dateOfBirth, int activity, string constring)
    {
        //pakeisti kai prideti menesio ir dienos pasirinkimai.(metodas sukurti YYYY-mm-dd string);
        string year = dateOfBirth.ToString();
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_update = DBManager.connection.CreateCommand();

            command_update.CommandText =
                "UPDATE user_data SET " +
                "height = @expr2, weight = @expr3, gender = @expr4, " +
                "goal = @expr5, physical_activity = @expr6, " +
                "date_of_birth = @expr7 " +
                "WHERE id_user = @expr1";


            IDbDataParameter p_id = command_update.CreateParameter();
            p_id.ParameterName = "@expr1"; p_id.Value = id;
            command_update.Parameters.Add(p_id);

            IDbDataParameter p_height = command_update.CreateParameter();
            p_height.ParameterName = "@expr2"; p_height.Value = height;
            command_update.Parameters.Add(p_height);

            IDbDataParameter p_weight = command_update.CreateParameter();
            p_weight.ParameterName = "@expr3"; p_weight.Value = weight;
            command_update.Parameters.Add(p_weight);

            IDbDataParameter p_gender = command_update.CreateParameter();
            p_gender.ParameterName = "@expr4"; p_gender.Value = gender;
            command_update.Parameters.Add(p_gender);

            IDbDataParameter p_goal = command_update.CreateParameter();
            p_goal.ParameterName = "@expr5"; p_goal.Value = goal;
            command_update.Parameters.Add(p_goal);

            IDbDataParameter p_activity = command_update.CreateParameter();
            p_activity.ParameterName = "@expr6"; p_activity.Value = activity;
            command_update.Parameters.Add(p_activity);

            IDbDataParameter p_dateOfBirth = command_update.CreateParameter();
            p_dateOfBirth.ParameterName = "@expr7"; p_dateOfBirth.Value = dateOfBirth;
            command_update.Parameters.Add(p_dateOfBirth);

            command_update.ExecuteNonQuery();
        }
        catch (SqliteException e) { System.Console.WriteLine(e.Message); }
        finally { DBManager.CloseConnection(); }
    }

    private string BuildString(string[] array)
    {
        int length = array.Length;
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            array[i].Replace(',', '.');
            sBuilder.Append(array[i]).Append(';');
        }
        Debug.Log(sBuilder.ToString());
        return sBuilder.ToString();
    }

    public bool CheckSurveyCompleted(int id, string constring)
    {
        string PlaceholderString = "0,00;0,00;Male;1;";
        bool result = false;

        try
        {
            DBManager.OpenConnection();
            IDbCommand command_check = DBManager.connection.CreateCommand();

            command_check.CommandText =
                "SELECT height, weight, gender, physical_activity " +
                "FROM user_data " +
                "WHERE id_user = @expr1;";


            IDbDataParameter p_id = command_check.CreateParameter();
            p_id.ParameterName = "@expr1"; p_id.Value = id;
            command_check.Parameters.Add(p_id);

            using (IDataReader reader = command_check.ExecuteReader())
            {
                if (reader.Read())
                {
                    string height = reader.GetValue(0).ToString();
                    string weight = reader.GetValue(1).ToString();
                    string gender = reader.GetValue(2).ToString();
                    string physicalActivity = reader.GetValue(3).ToString();
                    string[] array = { height, weight, gender, physicalActivity };
                    string currentString = BuildString(array);
                    result = (currentString == PlaceholderString) ? false : true;
                }

            }
            return result;
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        finally
        {
            DBManager.CloseConnection();
        }
    }

    public string ReturnUserData(int id, string constring)
    {
        string PlaceholderString = "0,00;0,00;Male;1;";
        string result = "";

        try
        {
            DBManager.OpenConnection();
            IDbCommand command_return = DBManager.connection.CreateCommand();
            command_return.CommandText =
                "SELECT id_user, height, weight, gender, goal, physical_activity, date_of_birth, creation_date FROM user_data WHERE id_user = @expr1;";

            IDbDataParameter p_id = command_return.CreateParameter();
            p_id.ParameterName = "@expr1"; p_id.Value = id;
            command_return.Parameters.Add(p_id);

            using (IDataReader reader = command_return.ExecuteReader())
            {
                if (reader.Read())
                {
                    string height = reader.GetValue(1).ToString();
                    string weight = reader.GetValue(2).ToString();
                    string gender = reader.GetValue(3).ToString();
                    string goal = reader.GetValue(4).ToString();
                    string physicalActivity = reader.GetValue(5).ToString();
                    string dateBirth = reader.GetValue(6).ToString();
                    string creationDate = reader.GetValue(7).ToString();
                    string[] array = { height, weight, gender, goal, physicalActivity, dateBirth, creationDate };
                    string currentString = BuildString(array);
                    result = currentString;
                }
            }
            return result;
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return "Error!";
        }

        finally
        {
            DBManager.CloseConnection();
        }
    }

    public string ReturnUsername(int id, string constring)
    {
        string result = "";

        try
        {
            DBManager.OpenConnection();
            IDbCommand command_return = DBManager.connection.CreateCommand();
            command_return.CommandText = "SELECT `user`.`username`" +
                " FROM `food_db`.`user` WHERE `user`.`id_user` = @expr1;";


            IDbDataParameter p_id = command_return.CreateParameter();
            p_id.ParameterName = "@expr1";
            p_id.Value = id;

            using (IDataReader reader = command_return.ExecuteReader())
            {
                if (reader.Read())
                {
                    string username = reader.GetValue(0).ToString();
                    result = username;
                }
            }
            return result;
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return "Error!";
        }
        finally
        {
            DBManager.CloseConnection();
        }

    }

    public List<FoodClass> ReturnFoodList(string constring)
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_return = DBManager.connection.CreateCommand();

            //!!!!
            command_return.CommandText = "SELECT * FROM v_meal_kcal_per_serving";
            List<FoodClass> foodlist = new List<FoodClass>();

            using (IDataReader reader = command_return.ExecuteReader())
            {
                while (reader.Read())
                {
                    string foodname = reader.GetValue(1).ToString();
                    double kcal = Convert.ToDouble(reader.GetValue(3));
                    foodlist.Add(new FoodClass(foodname, kcal));
                }

                return foodlist;
            }
        }
        catch (SqliteException e)
        {
            Debug.unityLogger.Log(e.Message);
            return new List<FoodClass>();
        }
        finally
        {
            DBManager.CloseConnection();
        }

    }

    public bool CheckIfUserExists(string username, string constring)
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_check = DBManager.connection.CreateCommand();
            command_check.CommandText = "SELECT COUNT(*) FROM user WHERE username = @expr1;";

            IDbDataParameter p_username = command_check.CreateParameter();
            p_username.ParameterName = "@expr1"; p_username.Value = username;
            command_check.Parameters.Add(p_username);

            int count = Convert.ToInt32(command_check.ExecuteScalar());
            return count > 0;
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        finally
        {
            DBManager.CloseConnection();
        }
    }

}
