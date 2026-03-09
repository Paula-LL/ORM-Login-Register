using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [Header(" UI ")]
    public TMP_InputField inputUsername;
    public TMP_InputField inputPassword;
    public Button registerButton;
    public TextMeshProUGUI messageText;

    [Header(" Game Objects ")]
    //public GameObject mainPagePanel;
    public GameObject registerPanel;



    private void Start() { 
        registerButton.onClick.AddListener(RegisterUser);
        messageText.text = "Input Username and password";
    }

    private void RegisterUser() {
        string user = inputUsername.text;
        string password = inputPassword.text;

        if (password.Length < 8) {
            messageText.text = "Password must have at lest 8 characters";
            return;
        }

        if (string.IsNullOrEmpty(user)) {
            messageText.text = "Username cannot be empty";
            return;
        }

        string databaseUri = "URI=file:" + DBCommons.databasePath ;

        try {
            using (var connectToDB = new SqliteConnection(databaseUri)) {
                connectToDB.Open();

                string consultDB = "INSERT INTO Users (UserName, Password) VALUES (@user, @password)";

                using (var command = new SqliteCommand(consultDB, connectToDB)) {
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@password", password);

                    try {

                        command.ExecuteNonQuery();
                        messageText.text = "User registered";

                        inputUsername.text = "";
                        inputPassword.text = "";

                        SceneManager.LoadScene("ShopScene");

                        /*if (mainPagePanel != null) { 
                            mainPagePanel.SetActive(true);
                        }

                        if (registerPanel != null) { 
                            mainPagePanel.SetActive(true);
                        }*/

                    } catch (SqliteException e) {
                        if (e.Message.Contains("UNIQUE"))
                        {
                            messageText.text = "Username already exists";
                        }
                        else {
                            messageText.text = "User register error";
                        }
                    }
                }
            }
        }
        catch (System.Exception e) {
            messageText.text = "Database connection error";
            Debug.LogError("Sqlite: " + e.Message);
        }
    }
}
