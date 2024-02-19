using System;
using System.Data;
using System.Collections.Generic;
using MySqlConnector;
public class DatabaseMethods
{
	public int Login(string username, string password, string constring, out int id)
	{
        MySqlCommand command = new MySqlCommand();
        MySqlConnection ConnectionObject = new MySqlConnection();
        ConnectionObject.ConnectionString = constring;
        id = -1;
        try
        {
            command.CommandText = "SELECT * FROM mitybosdb.naudotojas WHERE " +
                                    "naudotojo_vardas = @expr1 AND slaptazodis = @expr2";
            command.Parameters.Add("@expr1", MySqlDbType.VarChar, 50).Value = username;
            command.Parameters.Add("@expr2", MySqlDbType.VarChar, 255).Value = password;
            command.Connection = ConnectionObject;
            ConnectionObject.Open();
            using (var reader = command.ExecuteReader())
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
}
