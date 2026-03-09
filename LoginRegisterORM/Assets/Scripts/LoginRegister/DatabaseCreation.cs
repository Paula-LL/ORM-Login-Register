using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

[DefaultExecutionOrder(-100)]
public class DatabaseCreation : MonoBehaviour
{
    [Header(" Database Configuration ")]
    bool resetDataOnStart = false;

    private void Start()
    {
        CreateDatabase();    
    }

    private void CreateDatabase() {
        string databasePath = DBCommons.databasePath;

        if(!Directory.Exists(DBCommons.databaseDirectory))
        {
            Directory.CreateDirectory(DBCommons.databaseDirectory);
        }

        if (resetDataOnStart && File.Exists(databasePath)) { 
            File.Delete(databasePath);
        }

        /*if (!File.Exists(databasePath)) { 
            File.Create(databasePath).Close();
        }*/

        string databaseFile = "URI=file:" + databasePath;

        IDbConnection databaseConnection = new SqliteConnection(databaseFile);
        databaseConnection.Open();

        var command = databaseConnection.CreateCommand();

        command.CommandText = "PRAGMA foreign_keys = ON;";

        /*IDbCommand databaseCommandTableCreation = databaseConnection.CreateCommand();
        databaseCommandTableCreation.CommandText =
                "CREATE TABLE IF NOT EXISTS Users(" +
                "UserID INTEGER PRIMARY KEY AUTOINCREMENT," +
                "UserName TEXT, " +
                "Password TEXT CHECK(length(password) >= 8))";

        databaseCommandTableCreation.ExecuteNonQuery();
        */
        IDbCommand databaseCommandInventoryTableCreation = databaseConnection.CreateCommand();
        databaseCommandInventoryTableCreation.CommandText =
                "CREATE TABLE IF NOT EXISTS Inventory(" +
                "InventoryID INTEGER PRIMARY KEY AUTOINCREMENT," +
                "UserID INTEGER NOT NULL, "+
                "FOREIGN KEY(UserID) REFERENCES Users(UserID))";

        databaseCommandInventoryTableCreation.ExecuteNonQuery();//

        IDbCommand databaseCommandItemTableCreation = databaseConnection.CreateCommand();//
        databaseCommandItemTableCreation.CommandText =
                "CREATE TABLE IF NOT EXISTS Item(" +
                "ItemID INTEGER PRIMARY KEY AUTOINCREMENT," +
                "ItemName VARCHAR(50) NOT NULL," +
                "ItemPrice INTEGER NOT NULL," +
                "ItemDesc VARCHAR(250) NOT NULL, " +
                "ItemImage IMAGE NOT NULL, " +
                "InventoryID INTEGER NOT NULL,"+
                "FOREIGN KEY(InventoryID) REFERENCES Inventory(InventoryID))";

        databaseCommandItemTableCreation.ExecuteNonQuery();//

        IDbCommand databaseCommandContainTableCreation = databaseConnection.CreateCommand();//
        databaseCommandContainTableCreation.CommandText =
                "CREATE TABLE IF NOT EXISTS Contains(" +
                "ItemID INTEGER," +
                "InventoryID INTEGER,"+
                "Primary Key(ItemID, InventoryID),"+
                "ItemNumber INTEGER," +
                "FOREIGN KEY(InventoryID) REFERENCES Inventory(InventoryID)," +
                "FOREIGN KEY(ItemID) REFERENCES Item(ItemID))";

        databaseCommandContainTableCreation.ExecuteNonQuery();//

        if (resetDataOnStart) { 
            IDbCommand databaseCommandDelete = databaseConnection.CreateCommand();
            databaseCommandDelete.CommandText = "DELETE FROM Users";
            databaseCommandDelete.ExecuteNonQuery();
            databaseCommandDelete.Dispose(); 
        }

        //databaseCommandTableCreation.Dispose();
        databaseCommandInventoryTableCreation.Dispose();//
        databaseCommandItemTableCreation.Dispose();//
        databaseCommandContainTableCreation.Dispose();

        databaseConnection.Close();
    }
}

/*
 DROP DATABASE IF EXISTS ACTIVITY_DAVID;
CREATE DATABASE IF NOT EXISTS ACTIVITY_DAVID;
USE ACTIVITY_DAVID;
CREATE TABLE IF NOT EXISTS Users(
UserID INTEGER PRIMARY KEY AUTO_INCREMENT,
UserName TEXT, 
Password TEXT CHECK(length(password) >= 8));


CREATE TABLE IF NOT EXISTS Inventory(
InventoryID INTEGER PRIMARY KEY AUTO_INCREMENT,
UserID INTEGER NOT NULL, 
FOREIGN KEY(UserID) REFERENCES Users(UserID));

CREATE TABLE IF NOT EXISTS Item(
ItemID INTEGER PRIMARY KEY AUTO_INCREMENT,
ItemName VARCHAR(50) NOT NULL,
ItemPrice INTEGER NOT NULL,
ItemDesc VARCHAR(250) NOT NULL, 
#ItemImage IMAGE NOT NULL,  
InventoryID INTEGER NOT NULL,
FOREIGN KEY(InventoryID) REFERENCES Inventory(InventoryID));

CREATE TABLE IF NOT EXISTS Contains( 
ItemID INTEGER,
InventoryID INTEGER,
ItemNumber INTEGER,
Primary Key(ItemID, InventoryID),
FOREIGN KEY(InventoryID) REFERENCES Inventory(InventoryID),
FOREIGN KEY(ItemID) REFERENCES Item(ItemID));
          
INSERT INTO Users VALUES(001, 'Admin', '123456789');           
INSERT INTO Users VALUES(002, 'Test1', '987654321');
INSERT INTO Users VALUES(003, 'Test2', '147258369');
          
SELECT * FROM Users WHERE UserID;
SELECT I.* FROM Contains C JOIN Inventory Inv JOIN Item I ON Inv.InventoryID = I.InventoryID AND C.ItemID = I.ItemID WHERE Inv.UserID=@userID;
*/