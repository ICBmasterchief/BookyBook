using BookyBook.Domain;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace BookyBook.Data;
public class UserData
{
    public List<User>? UsersList = new();
    private readonly string UserJsonPath;
    public UserData()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            UserJsonPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "BookyBook.Data", "Data.Users.json");
        } else {
            UserJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "BookyBook.Data", "Data.Users.json");
        }
        GetRegisteredUsers();
    }
    public void AddUser(User user)
    {
        UsersList.Add(user);
        SaveUserData();
    }
    public void GetRegisteredUsers()
    {
        try
        {
        string JsonUsers = File.ReadAllText(UserJsonPath);
        UsersList =  JsonSerializer.Deserialize<List<User>>(JsonUsers);
        } catch (System.Exception)
        {
            Console.WriteLine("ERROR TRYING ACCESS DATA");
        }
    }
    public void SaveUserData()
    {
        string JsonUsers = JsonSerializer.Serialize(UsersList, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(UserJsonPath, JsonUsers);
    }
}