using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BookyBook.Domain;
using BookyBook.Service;
using Spectre.Console;
using System.Text.RegularExpressions;
using System.Text.Json;
using BookyBook.Data;
using Microsoft.VisualBasic;
using System.Data.Common;

namespace BookyBook.Presentation;

public class MainMenu
{
    public bool Exit = false;
    public readonly UserService userService = new();
    public readonly BookService bookService = new();
    public readonly BorrowingService borrowingService = new();
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
        userService.userData.GetRegisteredUsers();
        bookService.bookData.GetRegisteredBooks();
        borrowingService.borrowingData.GetRegisteredBorrowings();
    }
    public void ShowMenu() 
    {
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
                AnsiConsole.MarkupLine($"Registration date: {userService.LoggedUser.RegistrationDate.ToString().Substring(0, userService.LoggedUser.RegistrationDate.ToString().Length - 7)}");
                AnsiConsole.MarkupLine($"Penalty fee: {userService.LoggedUser.PenaltyFee} $");
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
                LogIn();
                break;
            case "- Sign Up":
                SignUp();
                break;
            case "- Search for books":
                SearchForBooks();
                break;
            case "- Show library":
                ShowLibrary();
                break;
            case "- Donate a book":
                SearchBooks();
                break;
            case "- Borrow a book":
                BorrowBook();
                break;
            case "- Return a book":
                ReturnBook();
                break;
            case "- My Account":
                NumMenu = 3;
                break;
            case "- Current Borrowed books":
                CurrentBorrowedBooks();
                break;
            case "- Borrowed books history":
                BorrowedBooksHistory();
                break;
            case "- Pay penalty fee":
                PayPenaltyFee();
                //InitializeData();
                break;
            case "<-Back to menu":
                NumMenu = 2;
                break;
            case "- Log Out":
                userService.LoggedUser = new();
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
    public void LogIn()
    {
        AnsiConsole.MarkupLine("[green]Log in[/]");
        string logginEmail = AnsiConsole.Ask<String>("Email:").ToLower();
        string logginPassword = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
        if (userService.LoggInUser(logginEmail, logginPassword))
        {
            AnsiConsole.MarkupLine("[yellow]You are logging in.[/]");
            NumMenu = 2;
        }
        Thread.Sleep(2000);
    }
    public void SignUp()
    {
        AnsiConsole.MarkupLine("[green]Creating new user[/]");
        AnsiConsole.MarkupLine("");
        string name = AnsiConsole.Ask<String>("User Name:").ToLower();
        string email = AnsiConsole.Ask<String>("Email:").ToLower();
        //while (CheckEmail(email))
        //{
            
        //}
        string password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
        string checkPassword = AnsiConsole.Prompt(new TextPrompt<string>("Repeat Password:").Secret());
        userService.SignUpUser(name, email, password, checkPassword);
        Thread.Sleep(2000);
    }
    public void SearchForBooks()
    {
        AnsiConsole.MarkupLine("[green]Searching for books[/]");
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
    }
    public void ShowLibrary()
    {
        List<Book> listBooks = bookService.GetBookList();
        if (listBooks.Count != 0)
        {
            AnsiConsole.MarkupLine("[green]Library[/]");
            AnsiConsole.MarkupLine("");
            CreateBookTable(listBooks);
            AnsiConsole.Write(BookTable);
            AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("<-Back to menu"));
        } else {
            AnsiConsole.MarkupLine("[yellow]We don't have any books yet, sorry :([/]");
            Thread.Sleep(2000);
        }
    }
    public void SearchBooks()
    {
        AnsiConsole.MarkupLine("[green]Donating a book[/]");
        AnsiConsole.MarkupLine("");
        if (AnsiConsole.Confirm("Are you sure you want to donate a book?"))
        {
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
                    copies = AnsiConsole.Ask<int>("Copies to donate:");
                    AnsiConsole.MarkupLine("Thank you!");
                    bookService.DonateBook(title, author, "", 0, copies, 0);
                } else {
                    AnsiConsole.MarkupLine("Ok... :(");
                }
            } else {
                string genre = AnsiConsole.Ask<String>("Genre:").ToLower();
                int year = AnsiConsole.Ask<int>("Year:");
                copies = AnsiConsole.Ask<int>("Copies to donate:");
                decimal score = decimal.Parse(AnsiConsole.Ask<String>("Score:"));
                bookService.DonateBook(title, author, genre, year, copies, score);
                AnsiConsole.MarkupLine("[yellow]Books added to our library.[/]");
                AnsiConsole.MarkupLine("Thank you!");
            }
        } else {
            AnsiConsole.MarkupLine("Ok... :(");
        }
        Thread.Sleep(2000);
    }
    public void BorrowBook()
    {
        AnsiConsole.MarkupLine("[green]Borrowing a book[/]");
        AnsiConsole.MarkupLine("");
        if (userService.LoggedUser.PenaltyFee > 0)
        {
            AnsiConsole.MarkupLine("[yellow]You can't borrow a book until you pay your penalty fee.[/]");
        } else {
            int borrowIdBook = AnsiConsole.Ask<int>("Book ID to borrow:");
            if (bookService.CheckExistingBookDataById(borrowIdBook))
            {
                List<Borrowing> borrowingList = borrowingService.HasActiveBorrowings(userService.LoggedUser.IdNumber);
                if (borrowingList.Count == 0)
                { 
                    DoBorrowing(borrowIdBook);
                } else {
                    Borrowing? foundBorrowing = borrowingList.Find(x => x.BookId == borrowIdBook);
                    if (foundBorrowing == null)
                    {
                        DoBorrowing(borrowIdBook);
                    } else {
                        AnsiConsole.MarkupLine("[yellow]You already have this book.[/]");
                    }
                }
            } else {
                AnsiConsole.MarkupLine("[yellow]That's not a valid Book ID.[/]");
            }
        }
        Thread.Sleep(3000);
    }
    public void ReturnBook()
    {
        AnsiConsole.MarkupLine("[green]Returning a book[/]");
        AnsiConsole.MarkupLine("");
        List<Borrowing> borrowingList = borrowingService.HasActiveBorrowings(userService.LoggedUser.IdNumber);
        if (borrowingList.Count != 0)
        {
            AnsiConsole.MarkupLine("[yellow]You have the following borrowed books:[/]");
            CreateBorrowingsTable(borrowingList);
            AnsiConsole.Write(BookTable);
            int borrowIdBook = AnsiConsole.Ask<int>("Book ID to return:");
            if (bookService.CheckExistingBookDataById(borrowIdBook))
            {
                Borrowing? foundBorrowing = borrowingList.Find(x => x.BookId == borrowIdBook);
                if (foundBorrowing != null)
                {
                    Book? book = bookService.bookData.BooksList.Find(x => x.IdNumber == borrowIdBook);
                    if (book != null)
                    {
                        foundBorrowing.Returned = true;
                        foundBorrowing.ReturnedDate = DateTime.Today;
                        if (foundBorrowing.ReturnedDate > foundBorrowing.DateToReturn)
                        {
                            TimeSpan difference = foundBorrowing.ReturnedDate.Value.Subtract(foundBorrowing.DateToReturn.Value);
                            double newPenalty = (double)difference.TotalDays * 1.25;
                            userService.LoggedUser.PenaltyFee += (decimal)newPenalty;
                            userService.UpdateLoggedUserPenalty();
                            borrowingService.UpdateBorrowingPenalty(foundBorrowing.IdNumber, (decimal)newPenalty);
                            AnsiConsole.MarkupLine($"[yellow]A penalty fee of[/] [red]{newPenalty}$[/] [yellow]has been added to your account[/]");
                        }
                        book.Copies += 1;
                        borrowingService.borrowingData.SaveBorrowingData();
                        bookService.bookData.SaveBookData();
                        userService.userData.SaveUserData();
                        AnsiConsole.MarkupLine($"[yellow]'{book.Title}' returned succesfully![/]");
                    } else {
                        //ERROR
                        AnsiConsole.MarkupLine("[red]ERROR in 'Return a book', book = null[/]");
                    }
                } else {
                    AnsiConsole.MarkupLine("[yellow]You don't have this book borrowed.[/]");
                }
            } else {
                AnsiConsole.MarkupLine("[yellow]That's not a valid Book ID.[/]");
            }
        } else {
            AnsiConsole.MarkupLine("[yellow]You don't have any books borrowed yet.[/]");
        }
        Thread.Sleep(3000);
    }
    public void CurrentBorrowedBooks()
    {
        List<Borrowing> borrowing_List = borrowingService.HasActiveBorrowings(userService.LoggedUser.IdNumber);
        if (borrowing_List.Count != 0)
        {
            AnsiConsole.MarkupLine("[yellow]You have the following borrowed books currently:[/]");
            CreateBorrowingsTable(borrowing_List);
            AnsiConsole.Write(BookTable);
            AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("<-Back to menu"));
        } else {
            AnsiConsole.MarkupLine("[yellow]You don't have any borrowed books yet.[/]");
            Thread.Sleep(3000);
        }
    }
    public void BorrowedBooksHistory()
    {
        List<Borrowing> borrowing__List = borrowingService.borrowingData.BorrowingsList.FindAll(x => x.UserId == userService.LoggedUser.IdNumber);
        if (borrowing__List.Count != 0)
        {
            AnsiConsole.MarkupLine("[yellow]This is your borrowed books history:[/]");
            CreateBorrowingsTable(borrowing__List);
            AnsiConsole.Write(BookTable);
            AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("<-Back to menu"));
        } else {
            AnsiConsole.MarkupLine("[yellow]You don't have any borrowed books yet.[/]");
            Thread.Sleep(3000);
        }
    }
    public void PayPenaltyFee()
    {
        decimal penalty = userService.LoggedUser.PenaltyFee;
        if (penalty > 0)
        {
            AnsiConsole.MarkupLine($"You have [yellow]{penalty}$[/] of penalty fee.");
            if (AnsiConsole.Confirm("Do you want to pay it?"))
            {
                AnsiConsole.MarkupLine("[yellow]Congratulations! You have paid your penalty fee. :)[/]");
                userService.LoggedUser.PenaltyFee = 0;
                userService.UpdateLoggedUserPenalty();
                userService.userData.SaveUserData();
            } else {
                AnsiConsole.MarkupLine("Ok, but remember that you wont be able to borrow more books");
                AnsiConsole.MarkupLine("until you pay your penalty fee.");
            }
        } else {
            AnsiConsole.MarkupLine("[yellow]You don't have any penalties to pay.[/]");
        }
        Thread.Sleep(3000);
    }






    public bool CheckEmail(string email)
    {
        string emailPatron = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        Regex regex = new Regex(emailPatron);
        return regex.IsMatch(email);
    }
    public void CreateBookTable(List<Book> bookList)
    {
        BookTable = new ();
        BookTable.AddColumns("ID", "Title", "Author", "Genre", "Year", "Copies", "Score");
        foreach (Book book in bookList)
            {
                BookTable.AddRow(book.IdNumber.ToString(), book.Title, book.Author, book.Genre, book.Year.ToString(), book.Copies.ToString(), book.Score.ToString());
            }
    }
    public void CreateBorrowingsTable(List<Borrowing> borrowingList)
    {
        List<Book> booksBorrowedlist = new();
        BookTable = new ();
        BookTable.AddColumns("ID", "Title", "Author", "Borrowing Date", "Borrowing End Date", "Returned Date", "Penalty Fee");
        foreach (var borrowing in borrowingList)
        {
            foreach (var book in bookService.bookData.BooksList)
            {
                if (book.IdNumber == borrowing.BookId)
                {
                    string borrowingDate = borrowing.BorrowingDate.ToString().Substring(0, borrowing.BorrowingDate.ToString().Length - 7);
                    string dateToReturn = borrowing.DateToReturn.ToString().Substring(0, borrowing.DateToReturn.ToString().Length - 7);
                    string returnedDate = "Not returned yet.";
                    if (borrowing.ReturnedDate.HasValue){
                        returnedDate = borrowing.ReturnedDate.Value.ToString("dd/MM/yyyy");
                    }
                    BookTable.AddRow(book.IdNumber.ToString(), book.Title, book.Author, borrowingDate, dateToReturn, returnedDate, borrowing.PenaltyFee.ToString()+" $");
                    booksBorrowedlist.Add(book);
                }
            }
        }
    }
    public void DoBorrowing(int borrowIdBook)
    {
        Book? book = bookService.bookData.BooksList.Find(x => x.IdNumber == borrowIdBook);
        if (book != null)
        {
            if (book.Copies > 0)
            {
                book.Copies -= 1;
                bookService.bookData.SaveBookData();
                borrowingService.BorrowBook(userService.LoggedUser.IdNumber, borrowIdBook);
                AnsiConsole.MarkupLine($"[yellow]'{book.Title}' borrowed succesfully![/]");
            } else {
                AnsiConsole.MarkupLine("[yellow]We don't have copies of that book available, sorry :([/]");
            }
        } else {
            //ERROR
            AnsiConsole.MarkupLine("[red]ERROR in 'Borrow a book', book = null[/]");
        } 
    }
}

    