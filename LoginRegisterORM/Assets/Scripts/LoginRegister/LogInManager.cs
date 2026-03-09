using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LogInManager : MonoBehaviour
{

    public static LoggedUserCredentials credentials;

    [Header(" UI ")]
    public TMP_InputField inputUsername;
    public TMP_InputField inputPassword;
    public Button loginButton;
    public TextMeshProUGUI messageText;

    [Header(" Game Objects ")]
    //public GameObject mainPagePanel;
    public GameObject loginPanel;

    private void Start()
    {
        loginButton.onClick.AddListener(LoginCheck);
        messageText.text = "Input username and password";
    }

    private void LoginCheck()
    {
        credentials = new LoggedUserCredentials();
        string user = inputUsername.text.Trim();
        string password = inputPassword.text.Trim();

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
        {
            messageText.text = "Fill in the input fields";
            return;
        }

        string databaseURI = "URI=file:" + DBCommons.databasePath;

        try
        {
            using (var connectDB = new SqliteConnection(databaseURI))
            {
                connectDB.Open();

                string consultDB = "SELECT * FROM Users WHERE UserName=@user AND Password=@password";

                using (var commandDB = new SqliteCommand(consultDB, connectDB))
                {
                    commandDB.Parameters.AddWithValue("@user", user);
                    commandDB.Parameters.AddWithValue("@password", password);

                    using (IDataReader reader = commandDB.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            messageText.text = "LogIn is correct";

                            SceneManager.LoadScene("ShopScene");

                            credentials.userID = reader.GetInt32(0);
                            credentials.username = reader.GetString(1);



                            /*if (mainPagePanel != null)
                            {
                                mainPagePanel.SetActive(false);//
                            }

                            if (loginPanel != null)
                            {
                                mainPagePanel.SetActive(true);//
                            }*/
                        }
                        else
                        {
                            messageText.text = "Username or Password incorrect. Try again";
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            messageText.text = "Database connection error";
            Debug.LogError("Sqlite:" + e.Message);
        }
    }

    public class LoggedUserCredentials
    {
        public int userID;
        public string username;
    }

}
