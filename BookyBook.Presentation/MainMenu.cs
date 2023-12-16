using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BookyBook.Domain;
using BookyBook.Service;
using Spectre.Console;
using System.Text.Json;
using BookyBook.Data;
using Microsoft.VisualBasic;

namespace BookyBook.Presentation;

public class MainMenu
{
    public bool Exit = false;
    public readonly UserService userService = new();
    private string? Option;
    private int NumMenu = 1;
    private readonly SelectionPrompt<string> MainPrompt = new SelectionPrompt<string>()
        .Title("Main Menu")
        .PageSize(10)
        .AddChoices("Log in")
        .AddChoices("Sign Up")
        .AddChoices("Search for books")
        .AddChoices("Show book catalog")
        .AddChoices("Exit");
    private readonly SelectionPrompt<string> LoggedInPrompt = new SelectionPrompt<string>()
        .Title("Main Menu")
        .PageSize(10)
        .AddChoices("Search for books")
        .AddChoices("Show book catalog")
        .AddChoices("Borrow a book")
        .AddChoices("Return a book")
        .AddChoices("Donate a book")
        .AddChoices("Pay penalty fee")
        .AddChoices("My Account")
        .AddChoices("Exit");
    public void InitializeData()
    {
        userService.userData.GetRegisteredUsers();
    }
    public void ShowMenu() 
    {
        //DESCOMENTA LO DE DEBAJO AL FINAL
        AnsiConsole.Clear();
        ShowLogo();
        
        switch (NumMenu)
        {
            case 1:
                Option = AnsiConsole.Prompt(MainPrompt);
                break;
            case 2:
                LoggedInPrompt.Title($"Wellcome {userService.LoggedUser.Name}.");
                Option = AnsiConsole.Prompt(LoggedInPrompt);
                break;
            case 3:
                
                break;
        }
        

        

        

        ProcessOption(Option);
    }

    private void ProcessOption(string option)
    {
        switch (option)
        {
            case "Log in":
                string logginEmail = AnsiConsole.Ask<String>("Email:");
                string logginPassword = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
                if (userService.LoggInUser(logginEmail, logginPassword))
                {
                    AnsiConsole.MarkupLine("YOU ARE LOGGING IN");
                    NumMenu = 2;
                }
                Thread.Sleep(2000);
                AnsiConsole.Clear();
                break;
            case "Sign Up":
                string name = AnsiConsole.Ask<String>("User Name:");
                string email = AnsiConsole.Ask<String>("Email:");
                string password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
                string checkPassword = AnsiConsole.Prompt(new TextPrompt<string>("Repeat Password:").Secret());
                userService.SignUpUser(name, email, password, checkPassword);
                Thread.Sleep(2000);
                break;
            case "Search for books":
                AnsiConsole.MarkupLine("YOU ARE SEARCHING FOR BOOKS");
                Thread.Sleep(2000);
                AnsiConsole.Clear();
                break;
            case "Show book catalog":
                AnsiConsole.MarkupLine("HERE YOU HAVE ALL THE BOOKS");
                Thread.Sleep(2000);
                AnsiConsole.Clear();
                break;
            case "Exit":
                AnsiConsole.MarkupLine("EXITING THE APP IN 1 SECONDS");
                Thread.Sleep(2000);
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

    