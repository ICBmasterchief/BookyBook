using BookyBook.Domain;
using BookyBook.Service;
using Spectre.Console;

namespace BookyBook.Presentation;

public class MainMenu
{
    public bool Exit = false;
    public readonly BorrowingService borrowingService = new();
    public readonly UserService userService = new();
    public readonly BookService bookService = new();
    public readonly string machineName = Environment.GetEnvironmentVariable("MACHINE_NAME");
    private string? Option;
    private int NumMenu = 1;
    private Table BookTable = new();
    private readonly SelectionPrompt<string> MainPrompt = new SelectionPrompt<string>()
        .Title("MAIN MENU")
        .PageSize(10)
        .AddChoices("- Log in")
        .AddChoices("- Sign Up")
        .AddChoices("- Search for books")
        .AddChoices("- Show library")
        .AddChoices("- Exit");
    private readonly SelectionPrompt<string> LoggedInPrompt = new SelectionPrompt<string>()
        .PageSize(10)
        .AddChoices("- Search for books")
        .AddChoices("- Show library")
        .AddChoices("- Borrow a book")
        .AddChoices("- Return a book")
        .AddChoices("- Donate a book")
        .AddChoices("- My Account")
        .AddChoices("- Log Out")
        .AddChoices("- Exit");
    private readonly SelectionPrompt<string> AccountPrompt = new SelectionPrompt<string>()
        .PageSize(10)
        .AddChoices("- Pay penalty fee")
        .AddChoices("- Current Borrowed books")
        .AddChoices("- Borrowed books history")
        .AddChoices("<-Back to menu");
    public void InitializeData()
    {
        borrowingService.borrowingData.GetRegisteredBorrowings();
        userService.userData.GetRegisteredUsers();
        bookService.bookData.GetRegisteredBooks();
    }
    public void ShowMenu() 
    {
        AnsiConsole.Clear();
        ShowLogo();
        AnsiConsole.MarkupLine($"[grey]Name of this System/Computer: {machineName ?? "Undefined"}[/]");
        AnsiConsole.MarkupLine("");

        switch (NumMenu)
        {
            case 1:
                Option = AnsiConsole.Prompt(MainPrompt);
                break;
            case 2:
                LoggedInPrompt.Title($"WELLCOME [bold][green]{UserService.LoggedUser.Name}[/][/]");
                Option = AnsiConsole.Prompt(LoggedInPrompt);
                break;
            case 3:
                AnsiConsole.MarkupLine("[green]ACCOUNT MENU[/]");
                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine($"Name: {UserService.LoggedUser.Name}");
                AnsiConsole.MarkupLine($"Email: {UserService.LoggedUser.Email}");
                AnsiConsole.MarkupLine($"Registration date: {UserService.LoggedUser.RegistrationDate.ToString().Substring(0, UserService.LoggedUser.RegistrationDate.ToString().Length - 7)}");
                AnsiConsole.MarkupLine($"Penalty fee: {UserService.LoggedUser.PenaltyFee} $");
                AnsiConsole.MarkupLine("");
                Option = AnsiConsole.Prompt(AccountPrompt);
                break;
        }
        ProcessOption(Option);
    }

    private void ProcessOption(string? option)
    {
        switch (option)
        {
            case "- Log in":
                if (userService.LogIn())
                {
                    NumMenu = 2;
                }
                Thread.Sleep(2000);
                break;
            case "- Sign Up":
                borrowingService.userService.SignUp();
                break;
            case "- Search for books":
                bookService.SearchForBooks();
                break;
            case "- Show library":
                bookService.ShowLibrary();
                break;
            case "- Donate a book":
                bookService.DonateBook();
                break;
            case "- Borrow a book":
                borrowingService.BorrowBook();
                break;
            case "- Return a book":
                borrowingService.ReturnBook();
                break;
            case "- My Account":
                NumMenu = 3;
                break;
            case "- Current Borrowed books":
                borrowingService.CurrentBorrowedBooks();
                break;
            case "- Borrowed books history":
                borrowingService.BorrowedBooksHistory();
                break;
            case "- Pay penalty fee":
                borrowingService.PayPenaltyFee();
                break;
            case "<-Back to menu":
                NumMenu = 2;
                break;
            case "- Log Out":
                UserService.LoggedUser = new();
                NumMenu = 1;
                break;
            case "- Exit":
                AnsiConsole.MarkupLine("[yellow]EXITING THE APP IN 2 SECONDS[/]");
                Thread.Sleep(2000);
                AnsiConsole.Clear();
                Exit = true;
                break;
            default:
                AnsiConsole.MarkupLine("[red]Invalid option. Try again.[/]");
                Thread.Sleep(3000);
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
}

    