using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using TMPro;
using SQLite4Unity3d;

public class UsersManager : MonoBehaviour
{

    public Transform userScrollContainer;
    public Button showUsersButton;
    public GameObject userPrefabs;

    private DatabseService database; 

    private void Start()
    {
        database = new DatabseService(DBCommons.databasePath);
        showUsersButton.onClick.AddListener(LoadUsers);
        Debug.Log("BD usada per SQLite4Unity3d: " + DBCommons.databasePath);
    }

    public void LoadUsers()
    {

        Debug.Log("Carregant usuaris...");

        List<UserClass> users = database.GetAllUsers();

        Debug.Log("Usuaris trobats: " + users.Count);

        foreach (Transform child in userScrollContainer)
            Destroy(child.gameObject);

        foreach (var user in users)
        {
            Debug.Log("Usuari: " + user.Username);
            ShowUsernames(user);
        }
    }

    private void ShowUsernames(UserClass user)
    {

        GameObject entry = Instantiate(userPrefabs, userScrollContainer);

        TextMeshProUGUI nameText = entry.transform.Find("UserNameText").GetComponent<TextMeshProUGUI>();
        nameText.text = user.Username;

        Button deleteButton = entry.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() =>
        {
            database.DeleteUser(user.UserID);
            LoadUsers();
        });

    }
}

/*
 
public Transform userScrollContainer;
    public Button showUsersButton;
    public GameObject userPrefabs;

    private string dbPath;

    private void Start()
    {
        dbPath = "URI=file:" + DBCommons.databasePath;
        showUsersButton.onClick.AddListener(LoadUsers);
    }

    public void LoadUsers() {
        foreach (Transform child in userScrollContainer)
        {
            Destroy(child.gameObject);
        }

        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            string query = "SELECT UserID, UserName FROM Users";

            using (var cmd = new SqliteCommand(query, conn))
            using (IDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int userID = reader.GetInt32(0);
                    string username = reader.GetString(1);

                    ShowUsernames(userID, username);
                }
            }
        }
    }

    private void ShowUsernames(int userID, string username) {

        GameObject entry = Instantiate(userPrefabs, userScrollContainer);

        TextMeshProUGUI usernameText = entry.transform.Find("UsernameText").GetComponent<TextMeshProUGUI>();
        usernameText.text = username;
        Button deleteButton = entry.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => DeleteUser(userID));

    }

    private void DeleteUser(int userID) {

        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            string query = "DELETE FROM Users WHERE UserID=@id";

            using (var cmd = new SqliteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", userID);
                cmd.ExecuteNonQuery();
            }
        }

        LoadUsers();

    }
 
 */