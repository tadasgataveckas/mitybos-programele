using System;
using System.Data;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Mono.Data.Sqlite;

public class DatabaseMethods
{
    public int Login(string username, string password)
    {
        int id = -1;
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_login = DBManager.connection.CreateCommand();
            command_login.CommandText = "SELECT * FROM user WHERE " +
                                    "username = @expr1 AND password = @expr2;";

            // username parameter
            IDbDataParameter p_username = command_login.CreateParameter();
            p_username.ParameterName = "@expr1"; p_username.Value = username;
            command_login.Parameters.Add(p_username);

            // password parameter
            IDbDataParameter p_password = command_login.CreateParameter();
            p_password.ParameterName = "@expr2"; p_password.Value = password;
            command_login.Parameters.Add(p_password);

            using (IDataReader reader = command_login.ExecuteReader())
            {
                if (reader.Read())
                {
                    id = int.Parse(reader[0].ToString());
                    return id;
                }

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

    public bool RegisterUser(string email, string username, string password)
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
            return rowCount > 0;

        }
        catch (SqliteException e)
        {
            System.Console.WriteLine(e.Message); return false;
        }
        finally { DBManager.CloseConnection(); }
    }

    public void InsertUserData(UserData userData)
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_insert = DBManager.connection.CreateCommand();
            command_insert.CommandText =
                "INSERT INTO user_data" +
                "(id_user, height, weight, gender, goal, physical_activity, date_of_birth)" +
                " VALUES("
                + userData.id_user + ", "
                + userData.height + ", "
                + userData.weight + ", " +
                "'" + userData.GetGenderString() + "', " +
                "'" + userData.GetGoalString() + "', "
                + userData.physical_activity + ", '"
                + userData.date_of_birth + "');";

            command_insert.ExecuteNonQuery();
        }
        catch (SqliteException e)
        {
            System.Console.WriteLine(e.Message);
        }
        finally { DBManager.CloseConnection(); }
    }

    public bool InsertUserAllergy(int id_user, int id_allergy)
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_insert = DBManager.connection.CreateCommand();
            command_insert.CommandText =
                "INSERT INTO user_selected_allergies" +
                "(id_user, id_allergy)" +
                " VALUES(" + id_user + ", " + id_allergy + ");";

            return command_insert.ExecuteNonQuery() > 0;
        }
        catch (SqliteException e)
        {
            System.Console.WriteLine(e.Message); return false;
        }
        finally { DBManager.CloseConnection(); }
    }

    public void UpdateUserData(UserData userData)
    {
        //pakeisti kai prideti menesio ir dienos pasirinkimai.(metodas sukurti YYYY-mm-dd string);
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
            p_id.ParameterName = "@expr1"; p_id.Value = userData.id_user;
            command_update.Parameters.Add(p_id);

            IDbDataParameter p_height = command_update.CreateParameter();
            p_height.ParameterName = "@expr2"; p_height.Value = userData.height;
            command_update.Parameters.Add(p_height);

            IDbDataParameter p_weight = command_update.CreateParameter();
            p_weight.ParameterName = "@expr3"; p_weight.Value = userData.weight;
            command_update.Parameters.Add(p_weight);

            IDbDataParameter p_gender = command_update.CreateParameter();
            p_gender.ParameterName = "@expr4"; p_gender.Value = userData.GetGenderString();
            command_update.Parameters.Add(p_gender);

            IDbDataParameter p_goal = command_update.CreateParameter();
            p_goal.ParameterName = "@expr5"; p_goal.Value = userData.GetGoalString();
            command_update.Parameters.Add(p_goal);

            IDbDataParameter p_activity = command_update.CreateParameter();
            p_activity.ParameterName = "@expr6"; p_activity.Value = userData.physical_activity;
            command_update.Parameters.Add(p_activity);

            IDbDataParameter p_dateOfBirth = command_update.CreateParameter();
            p_dateOfBirth.ParameterName = "@expr7"; p_dateOfBirth.Value = userData.date_of_birth;
            command_update.Parameters.Add(p_dateOfBirth);

            command_update.ExecuteNonQuery();
        }
        catch (SqliteException e) { System.Console.WriteLine(e.Message); }
        finally { DBManager.CloseConnection(); }
    }

    public bool CheckIfSurveyCompleted(int id_user)
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_check = DBManager.connection.CreateCommand();
            command_check.CommandText =
                "SELECT COUNT(*) FROM user_data WHERE id_user = " + id_user + ";";

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

    public string ReturnUsername(int id)
    {
        string result = "";

        try
        {
            DBManager.OpenConnection();
            IDbCommand command_return = DBManager.connection.CreateCommand();
            command_return.CommandText = "SELECT username FROM user WHERE id_user = @expr1;";


            IDbDataParameter p_id = command_return.CreateParameter();
            p_id.ParameterName = "@expr1"; p_id.Value = id;
            command_return.Parameters.Add(p_id);


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

    public List<FoodClass> ReturnFoodList()
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

    public bool IsUsernameTaken(string username)
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

    public bool IsEmailInUse(string email)
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_check = DBManager.connection.CreateCommand();
            command_check.CommandText = "SELECT COUNT(*) FROM user WHERE email = @expr1;";

            IDbDataParameter p_email = command_check.CreateParameter();
            p_email.ParameterName = "@expr1"; p_email.Value = email;
            command_check.Parameters.Add(p_email);

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

    public bool IsPasswordCorrect(string username, string password)
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_check = DBManager.connection.CreateCommand();
            command_check.CommandText = "SELECT COUNT(*) FROM user WHERE username = @expr1 AND password = @expr2;";

            IDbDataParameter p_username = command_check.CreateParameter();
            p_username.ParameterName = "@expr1"; p_username.Value = username;
            command_check.Parameters.Add(p_username);

            IDbDataParameter p_password = command_check.CreateParameter();
            p_password.ParameterName = "@expr2"; p_password.Value = password;
            command_check.Parameters.Add(p_password);

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
