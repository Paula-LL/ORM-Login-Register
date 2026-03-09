using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UsersManager;

public class DatabseService
{
    private SQLiteConnection _connection;

    public DatabseService(string databasePath)
    {
        _connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        _connection.CreateTable<UserClass>();
    }

    public List<UserClass> GetAllUsers()
    {
        return _connection.Table<UserClass>().ToList();
    }

    public void DeleteUser(int id)
    {
        _connection.Delete<UserClass>(id);
    }
}
