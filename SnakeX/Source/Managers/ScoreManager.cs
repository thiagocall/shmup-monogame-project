using System;
using System.Data.SQLite;

public class ScoreService
{
    private readonly string _dbFilePath = "../../score_database.db";
    private readonly string _password = "navinha";

    public ScoreService()
    {
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        // Verificar se o arquivo já existe
        if (!System.IO.File.Exists(_dbFilePath))
        {
            SQLiteConnection.CreateFile(_dbFilePath);
            using (var connection = GetConnection())
            {
                connection.Open();
                // Criar tabela para salvar os scores
                using (var command = new SQLiteCommand(
                    "CREATE TABLE Scores (Id INTEGER PRIMARY KEY AUTOINCREMENT, PlayerName TEXT, Score INTEGER, DateAchieved TEXT);", 
                    connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    private SQLiteConnection GetConnection()
    {
        var connection = new SQLiteConnection($"Data Source={_dbFilePath};Version=3;");
        // connection.SetPassword(_password); // Configurar a senha do banco
        return connection;
    }

    public void SaveScore(string playerName, int score)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            using (var command = new SQLiteCommand(
                "INSERT INTO [Scores] (PlayerName, Score, DateAchieved) VALUES (@PlayerName, @Score, @DateAchieved);", 
                connection))
            {
                command.Parameters.AddWithValue("@PlayerName", playerName);
                command.Parameters.AddWithValue("@Score", score);
                command.Parameters.AddWithValue("@DateAchieved", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }
    }

    public long GetHiScore()
    {
        long result;
        using (var connection = GetConnection())
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT max(Score) FROM Scores;", connection))
            result = (long)command.ExecuteScalar();
        }

        return result;
    }
}