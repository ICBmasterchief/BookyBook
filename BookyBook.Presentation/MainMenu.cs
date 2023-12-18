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
    private Table BookTable = new();
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
                string logginEmail = AnsiConsole.Ask<String>("Email:").ToLower();
                string logginPassword = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
                if (userService.LoggInUser(logginEmail, logginPassword))
                {
                    AnsiConsole.MarkupLine("[yellow]You are logging in.[/]");
                    NumMenu = 2;
                }
                Thread.Sleep(2000);
                AnsiConsole.Clear();
                break;
            case "- Sign Up":
                AnsiConsole.MarkupLine("[yellow]Creating new user.[/]");
                AnsiConsole.MarkupLine("");
                string name = AnsiConsole.Ask<String>("User Name:").ToLower();
                string email = AnsiConsole.Ask<String>("Email:").ToLower();
                string password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
                string checkPassword = AnsiConsole.Prompt(new TextPrompt<string>("Repeat Password:").Secret());
                userService.SignUpUser(name, email, password, checkPassword);
                Thread.Sleep(2000);
                break;
            case "- Search for books":
                /*AnsiConsole.MarkupLine("YOU ARE SEARCHING FOR BOOKS");
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
                }*/
                AnsiConsole.MarkupLine("[yellow]Searching for books.[/]");
                AnsiConsole.MarkupLine("");
                string searchText = AnsiConsole.Ask<String>("Write book title or author:").ToLower();
                AnsiConsole.WriteLine("");
                List<Book> findedBooks = bookService.SearchBooks(searchText);
                if (findedBooks.Count != 0)
                {
                    CreateBookTable(findedBooks);
                    AnsiConsole.Write(BookTable);
                    AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("<-Back to menu"));
                } else {
                    AnsiConsole.MarkupLine("[yellow]No books found, sorry :([/]");
                    Thread.Sleep(2000);
                }
                AnsiConsole.Clear();
                break;
            case "- Show book catalog":
                List<Book> listBooks = bookService.GetBookList();
                if (listBooks.Count != 0)
                {
                    AnsiConsole.MarkupLine("[green]Book catalog:[/]");
                    AnsiConsole.MarkupLine("");
                    CreateBookTable(listBooks);
                    AnsiConsole.Write(BookTable);
                    AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("<-Back to menu"));
                } else {
                    AnsiConsole.MarkupLine("[yellow]We don't have any books yet, sorry :([/]");
                    Thread.Sleep(2000);
                }
                AnsiConsole.Clear();
                break;
            case "- Donate a book":
                AnsiConsole.MarkupLine("[yellow]What book do you want to donate?[/]");
                AnsiConsole.MarkupLine("");
                string title = AnsiConsole.Ask<String>("Book title:").ToLower();
                string author = AnsiConsole.Ask<String>("Author:").ToLower();
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
                    string genre = AnsiConsole.Ask<String>("Genre:").ToLower();
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

    public void ShowLogo()
    {
        AnsiConsole.Write(
        new FigletText("BookyBook")
            .LeftJustified()
            .Color(Color.Red));
        AnsiConsole.MarkupLine("[red] _____________________________________________________________________[/]");
        AnsiConsole.MarkupLine("[red] _____________________________________________________________________[/]");
        AnsiConsole.MarkupLine("");
    }
    public void CreateBookTable(List<Book> bookList)
    {
        BookTable = new ();
        BookTable.AddColumns("Title", "Author", "Genre", "Year", "Copies", "Score");
        foreach (Book book in bookList)
            {
                BookTable.AddRow(book.Title, book.Author, book.Genre, book.Year.ToString(), book.Copies.ToString(), book.Score.ToString());
            }
    }
}

    