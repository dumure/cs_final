using System.Text.Json;

internal class UsersDatabase
{
    private List<User> _users;

    public UsersDatabase()
    {
        _users = new List<User>();
    }
    private void Serialize()
    {
        using (FileStream file = new FileStream("users.json", FileMode.OpenOrCreate, FileAccess.Write))
        {
            JsonSerializer.Serialize(file, _users);
        }
    }
    private void Deserialize()
    {
        using (FileStream file = new FileStream("users.json", FileMode.OpenOrCreate, FileAccess.Read))
        {
            if (file.Length > 0)
            {
                _users = JsonSerializer.Deserialize<List<User>>(file)!;
            }
        }
    }
    public List<User> GetUsers()
    {
        Deserialize();
        return _users;
    }
    public void AddUser(User user)
    {
        _users.Add(user);
        Serialize();
    }
}