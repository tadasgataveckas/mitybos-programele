using System;
using System.Collections.Generic;
using System.Data;

public class ScoresMagicExpedition
{
    public long id_score;
    public int id_user;
    public float score;
    public string score_date;

    public ScoresMagicExpedition(long id_score, int id_user, float score, string score_date)
    {
        this.id_score = id_score;
        this.id_user = id_user;
        this.score = score;
        this.score_date = score_date;
    }

    // saves score and returns inserted score id
    public static long SaveScore(int id_user, float score)
    {
        long id_score = -1;
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_save = DBManager.connection.CreateCommand();
            command_save.CommandText =
                "INSERT INTO scores_magic_expedition(id_user, score) " +
                "VALUES(" + id_user + ", " + score + "); " +
                "SELECT last_insert_rowid();";
            id_score = Convert.ToInt64(command_save.ExecuteScalar());
            return id_score;
        }
        catch (Exception e) { System.Console.WriteLine(e.Message); return id_score; }
        finally { DBManager.CloseConnection(); }
    }

    public static List<ScoresMagicExpedition> GetScoresById(int id_user)
    {
        List<ScoresMagicExpedition> list = new List<ScoresMagicExpedition>();
        try
        {
            DBManager.OpenConnection();
            IDbCommand command_get = DBManager.connection.CreateCommand();
            command_get.CommandText =
                "SELECT * FROM scores_magic_expedition" +
                " WHERE id_user = " + id_user +
                " ORDER BY score DESC, score_date DESC;";
            IDataReader reader = command_get.ExecuteReader();

            while (reader.Read())
            {
                long id_score = long.Parse(reader[0].ToString());
                float score = float.Parse(reader[2].ToString());
                string score_date = reader[3].ToString();

                ScoresMagicExpedition scores = new ScoresMagicExpedition(
                    id_score, id_user, score, score_date);

                list.Add(scores);
            }
            return list;
        }
        catch (Exception e) { System.Console.WriteLine(e.Message); return list; }
        finally { DBManager.CloseConnection(); }
    }
}
