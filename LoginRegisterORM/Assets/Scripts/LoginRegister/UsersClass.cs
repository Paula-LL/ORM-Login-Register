[System.Serializable]
public class UserClass
{
    public int UserID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public UserClass() { }

    public UserClass(int userId, string Username, string Password)
    {
        this.UserID = userId;
        this.Username = Username;
        this.Password = Password;
    }

    public override string ToString() => $"[{UserID}] {Username}";
}
