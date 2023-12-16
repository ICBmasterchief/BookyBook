using BookyBook.Domain;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Text.Json;

namespace BookyBook.Data;
public class UserData
{
    private User ActualUser = new();
    public List<User>? UsersList = new();

    public string UserJsonPath = @"..\BookyBook.Data\Data.Users.json";
    public void AddUser(User user)
    {
        UsersList.Add(user);
        var JsonUsers = JsonSerializer.Serialize(UsersList, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(UserJsonPath, JsonUsers);
    }

    public void GetRegisteredUsers()
    {
        try
        {
        string JsonUsers = File.ReadAllText(UserJsonPath);
        UsersList =  JsonSerializer.Deserialize<List<User>>(JsonUsers);
        }
        catch (System.Exception)
        {
            Console.WriteLine("ERROR");
            //throw;
        }
    }
}