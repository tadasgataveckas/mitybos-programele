using System;
using System.Data;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using static UnityEngine.UI.GridLayoutGroup;
using Mysqlx.Expr;
using Mysqlx.Crud;
using UnityEngine.Analytics;
public class DatabaseMethods
{
  
	public int Login(string username, string password, out int id, string constring)
	{
        
        MySqlCommand command_login = new MySqlCommand();
        MySqlConnection ConnectionObject = new MySqlConnection();
        ConnectionObject.ConnectionString = constring;
        id = -1;
        try
        {
            command_login.CommandText = "SELECT * FROM food_db.user WHERE " +
                                    "username = @expr1 AND password = @expr2";
            command_login.Parameters.Add("@expr1", MySqlDbType.VarChar, 100).Value = username;
            command_login.Parameters.Add("@expr2", MySqlDbType.VarChar, 255).Value = password;
            command_login.Connection = ConnectionObject;
            ConnectionObject.Open();
            using (MySqlDataReader reader = command_login.ExecuteReader())
            {

                if (reader.HasRows)
                {
                    reader.Read();
                    id = (int)reader.GetValue(1);
                    return id;
                }
                else
                    id = -1;
                return id;
            }

        }
        catch (MySqlException e)
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
            ConnectionObject.Close();
        }
    }

    public bool Register(string email, string username, string password, string constring)
    {
        MySqlCommand command_register = new MySqlCommand();
        MySqlCommand command_select = new MySqlCommand();
        MySqlConnection ConnectionObject = new MySqlConnection();
        ConnectionObject.ConnectionString = constring;
        try
        {
            command_register.CommandText = "INSERT INTO `food_db`.`user` " +
                                "(`email`,`username`,`password`) " +
                                "VALUES (@expr1, @expr2, @expr3)";
            command_register.Parameters.Add("@expr1", MySqlDbType.VarChar, 100).Value = email;
            command_register.Parameters.Add("@expr2", MySqlDbType.VarChar, 100).Value = username;
            command_register.Parameters.Add("@expr3", MySqlDbType.VarChar, 255).Value = password;

            command_select.CommandText = "SELECT `user`.`id_user` FROM `food_db`.`user`" +
                                        " WHERE username = @expr1 AND password = @expr2";
            command_select.Parameters.Add("@expr1", MySqlDbType.VarChar, 100).Value = username;
            command_select.Parameters.Add("@expr2", MySqlDbType.VarChar, 255).Value = password;

            

            command_register.Connection = ConnectionObject;
            command_select.Connection = ConnectionObject; 
            ConnectionObject.Open();

            int rowcount = command_register.ExecuteNonQuery();
            int id_usr_fk = -1;
            using (MySqlDataReader reader = command_select.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    id_usr_fk = (int)reader.GetValue(1);
                }
                else
                    id_usr_fk = -1;
            }

            if (rowcount > 0)
            {
                InsertRegisterPlaceholder(id_usr_fk, ConnectionObject);
                return true;
            }
            else return false;

        }
        catch (MySqlException e)
        {
            System.Console.WriteLine(e.Message); return false;
        }
        finally { ConnectionObject.Close(); }
    }

    public void InsertRegisterPlaceholder(int id, MySqlConnection ConnectionObject)
    {
        MySqlCommand command_insert = new MySqlCommand();
        try
        {
            command_insert.CommandText = "INSERT INTO `food_db`.`user_data` " +
                                        "(`id_user`,`height`,`weight`,`gender`,`date_of_birth`,`goal`)" +
                                        " VALUES(@expr1, 0.0, 0.0, 'Vyras', CURRENT_DATE, 'Lose weight')";
            command_insert.Parameters.Add("@expr1", MySqlDbType.Int16, 6).Value = id;
            command_insert.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public void UpdateProfile() { }
}
