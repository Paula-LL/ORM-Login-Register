using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class UsersList
{
    private readonly string _connectionString;

    public UsersList(string dbPath)
    {
        _connectionString = "URI=file:" + dbPath;
    }

    public void CrearTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS Users (" +
                    "UserID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "Username TEXT NOT NULL UNIQUE, " +
                    "Password TEXT NOT NULL CHECK (length(Password) >= 8));";
                command.ExecuteNonQuery();
            }
        }
    }

    public void EmptyTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Users;";
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Username';";
                command.ExecuteNonQuery();
            }
        }
    }

    public bool UserInsert(UserClass user, out string message)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            message = "El nom d'usuari no pot estar buit.";
            return false;
        }
        if (user.Password == null || user.Password.Length < 8)
        {
            message = "La contrasenya ha de tenir com a minim 8 caracters.";
            return false;
        }

        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "INSERT INTO Users (Username, Password) VALUES (@u, @p);", connection))
                {
                    command.Parameters.AddWithValue("@u", user.Username.Trim());
                    command.Parameters.AddWithValue("@p", user.Password.Trim());
                    command.ExecuteNonQuery();
                }
            }
            message = $"User '{user.Username}' has been registered.";
            return true;
        }
        catch (SqliteException ex) when (ex.Message.Contains("UNIQUE"))
        {
            message = $"User '{user.Username}' already exixts.";
            return false;
        }
        catch (System.Exception ex)
        {
            message = "User insert error: " + ex.Message;
            Debug.LogError("insert user: " + ex.Message);
            return false;
        }
    }

    public UserClass Authentication(string username, string passwrod)
    {
        try
        {
            using (var connect = new SqliteConnection(_connectionString))
            {
                connect.Open();
                using (var command = new SqliteCommand(
                    "SELECT UserID, Username, Password FROM Users WHERE Username=@u AND Password=@p LIMIT 1;", connect))
                {
                    command.Parameters.AddWithValue("@u", username.Trim());
                    command.Parameters.AddWithValue("@p", passwrod.Trim());
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return new UserClass(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("User authentication: " + ex.Message);
        }
        return null;
    }

    public List<UserClass> GetAllUsers()
    {
        var userList = new List<UserClass>();
        try
        {
            using (var connect = new SqliteConnection(_connectionString))
            {
                connect.Open();
                using (var command = new SqliteCommand(
                    "SELECT UserID, Username, Passwrod FROM Users ORDER BY UserID;", connect))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        userList.Add(new UserClass(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Get users: " + ex.Message);
        }
        return userList;
    }

    public bool EraseID(int UserID)
    {
        try
        {
            using (var connect = new SqliteConnection(_connectionString))
            {
                connect.Open();
                using (var command = new SqliteCommand("DELETE FROM Users WHERE UserID=@UserID;", connect))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Erase ID: " + ex.Message);
        }
        return false;
    }

    public bool EraseUsername(string Username)
    {
        try
        {
            using (var connect = new SqliteConnection(_connectionString))
            {
                connect.Open();
                using (var cmd = new SqliteCommand("DELETE FROM Users WHERE Username=@u;", connect))
                {
                    cmd.Parameters.AddWithValue("@u", Username.Trim());
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Erase Username: " + ex.Message);
        }
        return false;
    }

}
