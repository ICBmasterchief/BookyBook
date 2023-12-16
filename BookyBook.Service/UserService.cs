using BookyBook.Data;
using BookyBook.Domain;

namespace BookyBook.Service;
public class UserService
{
    public readonly UserData userData = new();
    public void SignUpUser(string name, string email, string password)
    {
        if (userData.UsersList.Count == 0)
        {
            User user = new(name, email, password);
            userData.AddUser(user);
        } else {
            int? num = userData.UsersList.Last().IdNumber;
            num++;
            User user = new(name, email, password, num);
            userData.AddUser(user);
        }        
    }
}
