using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using BookyBook.Data;
using BookyBook.Domain;
using Microsoft.VisualBasic;

namespace BookyBook.Service;
public class UserService
{
    public readonly UserData userData = new();
    public User LoggedUser = new();
    public void SignUpUser(string name, string email, string password, string checkPassword)
    {
        if(password == checkPassword)
        {
            if (userData.UsersList.Count == 0)
            {
                User user = new(name, email, password);
                userData.AddUser(user);
            } else if (CheckExistingUserData(email, null) == false)
            {
                    int num = userData.UsersList.Last().IdNumber;
                    num++;
                    User user = new(name, email, password, num);
                    userData.AddUser(user);
            }
            
        } else {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: Passwords do not match.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    public bool CheckExistingUserData(string? email, string? password, bool loggIn=false)
    {
        foreach (var user in userData.UsersList)
        {
            if (loggIn)
            {
                if (user.Email == email && user.Password == password)
                {
                    LoggedUser = user;
                    return true;
                }
            } else if (user.Email == email)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Email already in use.");
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }
        }
        return false;
    }
    public bool LoggInUser(string email, string password)
    {
        if (CheckExistingUserData(email, password, true))
        {
            return true;
        } else {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: Invalid Email or Password.");
            Console.ForegroundColor = ConsoleColor.White;
        }
        return false;
    }
}
