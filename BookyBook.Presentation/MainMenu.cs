using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BookyBook.Domain;
using BookyBook.Service;
using Spectre.Console;
using System.Text.Json;
using BookyBook.Data;

namespace BookyBook.Presentation;

public class MainMenu
{
    public bool Exit = false;
    public readonly UserService userService = new();
    //public readonly UserData userData = new();
    //private User ActualUser = new();
    //public List<User> UsersList = new();
    public void InitializeData()
    {
        userService.userData.GetRegisteredUsers();
    }
    public void ShowMainMenu()
    {
        //AnsiConsole.Clear();
        ShowLogo();
        var MainPrompt = new SelectionPrompt<string>()
                .Title("Main Menu")
                .PageSize(10)
                .AddChoices("Log in")
                .AddChoices("Sign Up")
                .AddChoices("Search for books")
                .AddChoices("Show book catalog")
                .AddChoices("Exit");

        //var LoggedInPrompt = new SelectionPrompt<string>()
        //        .Title("Main Menu")
        //        .PageSize(10)
        //        .AddChoices("Log in")
        //        .AddChoices("Sign Up")
        //        .AddChoices("Search for books")
        //        .AddChoices("Show book catalog")
        //        .AddChoices("Exit");

        var Option = AnsiConsole.Prompt(MainPrompt);

        ProcessOption(Option);
    }

    private void ProcessOption(string option)
    {
        switch (option)
        {
            case "Log in":
                // Lógica para iniciar sesión o registrarse
                //var text = AnsiConsole.Prompt(new TextPrompt<string>("YOU ARE LOGGING IN"));
                Console.WriteLine("");
                AnsiConsole.MarkupLine("YOU ARE LOGGING IN");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                break;
            case "Sign Up":
                string name = AnsiConsole.Ask<String>("User Name:");
                //AnsiConsole.Clear();
                //ShowLogo();
                string email = AnsiConsole.Ask<String>("Email:");
                //AnsiConsole.Clear();
                //ShowLogo();
                string password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
                //AnsiConsole.Clear();
                //ShowLogo();
                userService.SignUpUser(name, email, password);
                //ActualUser = new User(name, email, password);
                //UsersList.Add(ActualUser);
                //var jsonUsuarios = JsonSerializer.Serialize(UsersList, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                //File.WriteAllText(@"..\BookyBook.Data\Data.Users.json", jsonUsuarios);
                //AnsiConsole.MarkupLine(name);
                //AnsiConsole.MarkupLine(email);
                //AnsiConsole.MarkupLine(password);
                Thread.Sleep(1000);
                break;
            case "Search for books":
                // Lógica para buscar libros
                AnsiConsole.MarkupLine("YOU ARE SEARCHING FOR BOOKS");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                break;
            case "Show book catalog":
                // Lógica para ver catálogo de libros
                AnsiConsole.MarkupLine("HERE YOU HAVE ALL THE BOOKS");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                break;
            case "Exit":
                AnsiConsole.MarkupLine("EXITING THE APP IN 1 SECONDS");
                Thread.Sleep(1000);
                AnsiConsole.Clear();
                Exit = true;
                //Environment.Exit(0);
                break;
            default:
                AnsiConsole.MarkupLine("[red]Invalid option. Try again.[/]");
                break;
        }
    }

    private void ShowLogo()
    {
        AnsiConsole.Write(
        new FigletText("BookyBook")
        .LeftJustified()
        .Color(Color.Red));
        AnsiConsole.MarkupLine("[red] _____________________________________________________________________[/]");
        AnsiConsole.MarkupLine("[red] _____________________________________________________________________[/]");
        AnsiConsole.MarkupLine("");
    }
}

    