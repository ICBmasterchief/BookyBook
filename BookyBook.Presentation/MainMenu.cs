using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BookyBook.Domain;
using Spectre.Console;

namespace BookyBook.Presentation;
public class MainMenu
{
    public bool Exit = false;
    public void ShowMainMenu()
    {
        AnsiConsole.Clear();
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
                string email = AnsiConsole.Ask<String>("Email:");
                string password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
                AnsiConsole.MarkupLine(name);
                AnsiConsole.MarkupLine(email);
                AnsiConsole.MarkupLine(password);
                Thread.Sleep(5000);
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
}

    