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
    public readonly BookService bookService = new();
    private string? Option;
    private int NumMenu = 1;
    private readonly SelectionPrompt<string> MainPrompt = new SelectionPrompt<string>()
        .Title("MAIN MENU")
        .PageSize(10)
        .AddChoices("- Log in")
        .AddChoices("- Sign Up")
        .AddChoices("- Search for books")
        .AddChoices("- Show book catalog")
        .AddChoices("- Exit");
    private readonly SelectionPrompt<string> LoggedInPrompt = new SelectionPrompt<string>()
        .PageSize(10)
        .AddChoices("- Search for books")
        .AddChoices("- Show book catalog")
        .AddChoices("- Borrow a book")
        .AddChoices("- Return a book")
        .AddChoices("- Donate a book")
        .AddChoices("- My Account")
        .AddChoices("- Log Out")
        .AddChoices("- Exit");
    private readonly SelectionPrompt<string> AccountPrompt = new SelectionPrompt<string>()
        .PageSize(10)
        .AddChoices("- Pay penalty fee")
        .AddChoices("<-Back to menu");
    public void InitializeData()
    {
        userService.userData.GetRegisteredUsers();
        bookService.bookData.GetRegisteredBooks();
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
                LoggedInPrompt.Title($"WELLCOME [bold][green]{userService.LoggedUser.Name}[/][/]");
                Option = AnsiConsole.Prompt(LoggedInPrompt);
                break;
            case 3:
                AnsiConsole.MarkupLine("[green]ACCOUNT MENU[/]");
                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine($"Name: {userService.LoggedUser.Name}");
                AnsiConsole.MarkupLine($"Email: {userService.LoggedUser.Email}");
                AnsiConsole.MarkupLine($"Registration date: {userService.LoggedUser.RegistrationDate}");
                AnsiConsole.MarkupLine($"Penalty fee: {userService.LoggedUser.PenaltyFee} $");
                AnsiConsole.MarkupLine("");
                Option = AnsiConsole.Prompt(AccountPrompt);
                break;
        }
        

        

        

        ProcessOption(Option);
    }

    private void ProcessOption(string option)
    {
        switch (option)
        {
            case "- Log in":
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
            case "- Sign Up":
                string name = AnsiConsole.Ask<String>("User Name:");
                string email = AnsiConsole.Ask<String>("Email:");
                string password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
                string checkPassword = AnsiConsole.Prompt(new TextPrompt<string>("Repeat Password:").Secret());
                userService.SignUpUser(name, email, password, checkPassword);
                Thread.Sleep(2000);
                break;
            case "- Search for books":
                AnsiConsole.MarkupLine("YOU ARE SEARCHING FOR BOOKS");
                string searchTitle = AnsiConsole.Ask<String>("Book title:");
                string SearchAuthor = AnsiConsole.Ask<String>("Author:");
                if (bookService.SearchBook(searchTitle, SearchAuthor).Title != null)
                {
                    Book findedBook = bookService.SearchBook(searchTitle, SearchAuthor);
                    AnsiConsole.MarkupLine("");
                    AnsiConsole.MarkupLine("[yellow]Book Found:[/]");
                    AnsiConsole.MarkupLine("");
                    AnsiConsole.MarkupLine($"Title: {findedBook.Title}");
                    AnsiConsole.MarkupLine($"Author: {findedBook.Author}");
                    AnsiConsole.MarkupLine($"Genre: {findedBook.Genre}");
                    AnsiConsole.MarkupLine($"Year: {findedBook.Year}");
                    AnsiConsole.MarkupLine($"Copies: {findedBook.Copies}");
                    AnsiConsole.MarkupLine($"Score: {findedBook.Score}");
                    AnsiConsole.MarkupLine("");
                    AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("<-Back to menu"));
                } else {
                    AnsiConsole.MarkupLine("");
                    AnsiConsole.MarkupLine("[yellow]No books found, sorry :([/]");
                    Thread.Sleep(2000);
                }
                AnsiConsole.Clear();
                break;
            case "- Show book catalog":
                AnsiConsole.MarkupLine("HERE YOU HAVE ALL THE BOOKS");
                Thread.Sleep(2000);
                AnsiConsole.Clear();
                break;
            case "- Donate a book":
                string title = AnsiConsole.Ask<String>("Book title:");
                string author = AnsiConsole.Ask<String>("Author:");
                int copies;
                if (bookService.CheckExistingBookData(title, author))
                {
                    AnsiConsole.MarkupLine("[yellow]We already have this book.[/]");
                    if (AnsiConsole.Confirm("Do you want to add new copies?"))
                    {
                        copies = int.Parse(AnsiConsole.Ask<String>("Copies to donate:"));
                        AnsiConsole.MarkupLine("Thank you!");
                        bookService.DonateBook(title, author, "", 0, copies, 0);
                    } else {
                        AnsiConsole.MarkupLine("Ok... :(");
                    }
                } else {
                    string genre = AnsiConsole.Ask<String>("Genre:");
                    int year = int.Parse(AnsiConsole.Ask<String>("Year:"));
                    copies = int.Parse(AnsiConsole.Ask<String>("Copies to donate:"));
                    decimal score = decimal.Parse(AnsiConsole.Ask<String>("Score:"));
                    AnsiConsole.MarkupLine("[yellow]Books added to our library.[/]");
                    AnsiConsole.MarkupLine("Thank you!");
                    bookService.DonateBook(title, author, genre, year, copies, score);
                }
                Thread.Sleep(2000);
                break;
            case "- My Account":
                NumMenu = 3;
                break;
            case "<-Back to menu":
                NumMenu = 2;
                break;
            case "- Log Out":
                userService.LoggedUser = new();
                NumMenu = 1;
                break;
            case "- Exit":
                AnsiConsole.MarkupLine("EXITING THE APP IN 2 SECONDS");
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

    