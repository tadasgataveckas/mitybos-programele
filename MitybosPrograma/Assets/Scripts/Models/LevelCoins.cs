using Mono.Data.Sqlite;
using System;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime;

public class LevelCoins
{
    public int id_user;
    public int level;
    public int xp;
    public int coins;
    public int streak;


    public LevelCoins(int id)
    {
        id_user = id;
        level = 0;
        xp = 0;
        coins = 0;
        streak = 0;
    }

    /// <summary>
    /// Updates User level, xp and coins according to id_user
    /// </summary>
    public void SynchData()
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_synch = DBManager.connection.CreateCommand();
            command_synch.CommandText =
                "SELECT level, xp, coins, streak " +
                "FROM level_coins " +
                "WHERE id_user = " + id_user;
            IDataReader reader = command_synch.ExecuteReader();

            if (reader.Read())
            {
                level = int.Parse(reader[0].ToString());
                xp = int.Parse(reader[1].ToString());
                coins = int.Parse(reader[2].ToString());

                coins = int.Parse(reader[2].ToString()); ;
            }
        }
        catch (SqliteException e) { System.Console.WriteLine(e.Message); }
        finally { DBManager.CloseConnection(); }
    }

    public override string ToString()
    {
        string lines =
            "id_user = " + id_user + "\n" +
            "level = " + level + "\n" +
            "xp = " + xp + "\n" +
            "coins = " + coins + "\n" +
            "streak = " + streak;
        return lines;
    }
}
