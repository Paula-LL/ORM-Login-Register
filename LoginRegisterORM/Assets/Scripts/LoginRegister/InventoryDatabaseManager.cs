using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryDatabaseManager : MonoBehaviour
{
    public List<ItemScriptableObject> allItemScriptables; 
    public List<ItemScriptableObject> userInventory = new List<ItemScriptableObject>();

    private int userID; 
    private int inventoryID;

    public void Start()
    {
        userID = LogInManager.credentials.userID;
        LoadInventoryInfo();
    }

    private void LoadInventoryInfo()
    {
        string databaseURI = "URI=file:" + DBCommons.databasePath;
        //Sacar info de Items y Contains en el mismo try catch.
        try
        {
            using (var connectDB = new SqliteConnection(databaseURI))
            {
                connectDB.Open();

                //string consultDB = "SELECT I.* FROM Contains C JOIN Inventory Inv JOIN Item I ON Inv.InventoryID = I.InventoryID AND C.ItemID = I.ItemID WHERE Inv.UserID=@userID";
                string inventoryQery = "SELECT InventoryID FROM Inventory WHERE UserID=@userID"; 

                using (var commandDB = new SqliteCommand(inventoryQery, connectDB))
                {
                    commandDB.Parameters.AddWithValue("@userID", userID);

                    object result = commandDB.ExecuteScalar();
                   
                    if (result == null)
                    {
                        Debug.Log("No Inventory found. Generating Inventory.");
                        CreateInventory(connectDB);
                    }
                    else
                    {
                        inventoryID = int.Parse(result.ToString()); 
                    }
                }

                string itemsQuery = "SELECT I.* FROM Contains C JOIN Item I ON C.ItemID WHERE C.InventiryID=@inventoryID";

                using (var comandDB = new SqliteCommand(itemsQuery, connectDB)) {
                    comandDB.Parameters.AddWithValue("@inventoryID", inventoryID);

                    using (IDataReader reader = comandDB.ExecuteReader()) {
                        userInventory.Clear();
                        while (reader.Read()) {
                            int itemID = reader.GetInt32(0);
                            int itemNumber = reader.GetInt32(5);

                            ItemScriptableObject itemScrptable = allItemScriptables.Find(i=>i.itemID == itemID);

                            if (itemScrptable != null)
                            {
                                userInventory.Add(itemScrptable);
                            }
                            else {
                                Debug.LogError("item " + itemID + " not found."); 
                            }
                        }
                    }
                }
                Debug.Log("Items in Inventory: " + userInventory.Count); 
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Database connection error");
            Debug.LogError("Sqlite:" + e.Message);
        }

        //Lanzar una query a base de datos en busca del ID de inventario de nuestro usuario (Join entre tabla USER y INVENTORY)

        //Con este id guardado como variable: Lanzar una query a base de datos en busca de items del inventario de nuestro usuario (Join entre INVENTORY, CONTAINS e ITEM)


    }

    private void CreateInventory(SqliteConnection connectDB)
    {
        string queryInsert = "INSERT INTO Inventory (UserID) VALUES (@userID)";

        using (var commandDB = new SqliteCommand(queryInsert, connectDB)) {
            commandDB.Parameters.AddWithValue("@userID", userID);
            commandDB.ExecuteNonQuery();
        }

        string getID = "SELECT lastRowID";

        using (var commandDB = new SqliteCommand(getID, connectDB)) {
            inventoryID = int.Parse(commandDB.ExecuteScalar().ToString()); 
        }
    }

    public void AddItemToInventory(int itemID, int amount) {
        string databaseURI = "URI=file:" + DBCommons.databasePath;

        using (var connectDB = new SqliteConnection(databaseURI)) {
            connectDB.Open();

            string dbInsert = "INSERT OR REPLACE INTO Contains (ItemID, InventoryID, ItemNumber)" +
                "VALUES (@itemID, @inventoryID, COALESCE((SELECT ItemNumber FROM Contains"  +
                "WHERE ItemID = @itemID AND InventoryID=@inventoryID), 0) + @amount)";

            using (var commandDB = new SqliteCommand(dbInsert, connectDB)) {
                commandDB.Parameters.AddWithValue("@itemID", itemID);
                commandDB.Parameters.AddWithValue("@inventoryID", inventoryID);
                commandDB.Parameters.AddWithValue("@amount", amount);
                commandDB.ExecuteNonQuery();
            }
        }

        LoadInventoryInfo(); 
    }
}


