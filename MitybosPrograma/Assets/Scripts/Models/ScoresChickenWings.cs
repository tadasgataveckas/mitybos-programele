using System.Data;
using System;
using System.Collections.Generic;

public class ScoresChickenWings
{
    public long id_score;
    public int id_user;
    public float score;
    public string score_date;

    public ScoresChickenWings(long id_score, int id_user, float score, string score_date)
    {
        this.id_score = id_score;
        this.id_user = id_user;
        this.score = score;
        this.score_date = score_date;
    }

    public static void SaveScore(int id_user, float score)
    {
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_save = DBManager.connection.CreateCommand();
            command_save.CommandText =
                "INSERT INTO scores_chicken_wings(id_user, score) " +
                "VALUES(" + id_user + ", " + score + ");";
            command_save.ExecuteNonQuery();
        }
        catch (Exception e) { System.Console.WriteLine(e.Message); }
        finally { DBManager.CloseConnection(); }
    }

    public static List<ScoresChickenWings> GetScoresById(int id_user)
    {
        List<ScoresChickenWings> list = new List<ScoresChickenWings>();
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_get = DBManager.connection.CreateCommand();
            command_get.CommandText =
                "SELECT * FROM scores_chicken_wings" +
                " WHERE id_user = " + id_user + 
                " ORDER BY score DESC, score_date DESC;";
            IDataReader reader = command_get.ExecuteReader();

            while (reader.Read())
            {
                long id_score = long.Parse(reader[0].ToString());
                float score = float.Parse(reader[2].ToString());
                string score_date = reader[3].ToString();

                ScoresChickenWings scores = new ScoresChickenWings(
                    id_score, id_user, score, score_date);

                list.Add(scores);
            }
            return list;
        }
        catch (Exception e) { System.Console.WriteLine(e.Message); return list; }
        finally { DBManager.CloseConnection(); }
    }
}
