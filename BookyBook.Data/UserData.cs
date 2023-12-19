using BookyBook.Domain;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Text.Json;

namespace BookyBook.Data;
public class UserData
{
    public List<User>? UsersList = new();
    public string UserJsonPath = @"..\BookyBook.Data\Data.Users.json";
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
            //throw;
        }
    }
    public void SaveUserData()
    {
        string JsonUsers = JsonSerializer.Serialize(UsersList, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(UserJsonPath, JsonUsers);
    }
}