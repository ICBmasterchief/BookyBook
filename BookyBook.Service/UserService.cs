using System.Runtime.CompilerServices;
using BookyBook.Data;
using BookyBook.Domain;
using Microsoft.VisualBasic;

namespace BookyBook.Service;
public class UserService
{
    public readonly UserData userData = new();
    public void SignUpUser(string name, string email, string password, string checkPassword)
    {
        if(password == checkPassword)
        {
            if (userData.UsersList.Count == 0)
            {
                User user = new(name, email, password);
                userData.AddUser(user);
            } else {
                if (CheckExistingData(name, email) == false)
                {
                    int? num = userData.UsersList.Last().IdNumber;
                    num++;
                    User user = new(name, email, password, num);
                    userData.AddUser(user);
                }
            }
        } else {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: Passwords do not match.");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);
        }
    }
    public bool CheckExistingData(string name, string email)
    {
        foreach (var item in userData.UsersList)
        {
            if (item.Name == name)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Name already in use.");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(1000);
                return true;
            } else if (item.Email == email)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Email already in use.");
                Console.ForegroundColor = ConsoleColor.White;
                {
                    
                };
                Thread.Sleep(1000);
                return true;
            }
        }
        return false;
    }
}
