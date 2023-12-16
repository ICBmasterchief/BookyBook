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
    public void InitializeData()
    {
        userService.userData.GetRegisteredUsers();
    }
    public void ShowMainMenu()
    {
        //DESCOMENTA LO DE DEBAJO AL FINAL
        AnsiConsole.Clear();
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
                Console.WriteLine("");
                AnsiConsole.MarkupLine("YOU ARE LOGGING IN");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                break;
            case "Sign Up":
                string name = AnsiConsole.Ask<String>("User Name:");
                string email = AnsiConsole.Ask<String>("Email:");
                string password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
                string checkPassword = AnsiConsole.Prompt(new TextPrompt<string>("Repeat Password:").Secret());
                userService.SignUpUser(name, email, password, checkPassword);
                Thread.Sleep(1000);
                break;
            case "Search for books":
                AnsiConsole.MarkupLine("YOU ARE SEARCHING FOR BOOKS");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                break;
            case "Show book catalog":
                AnsiConsole.MarkupLine("HERE YOU HAVE ALL THE BOOKS");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                break;
            case "Exit":
                AnsiConsole.MarkupLine("EXITING THE APP IN 1 SECONDS");
                Thread.Sleep(1000);
                AnsiConsole.Clear();
                Exit = true;
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

    