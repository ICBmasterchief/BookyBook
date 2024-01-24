using BookyBook.Data;
using BookyBook.Domain;
using Spectre.Console;

#pragma warning restore format
namespace BookyBook.Service;
public class BorrowingService
{
    public readonly BorrowingData borrowingData = new();
    public readonly UserService userService = new();
    public readonly BookService bookService = new();
    public Table BookTable = new();
    public void BorrowBook()
    {
        AnsiConsole.MarkupLine("[green]Borrowing a book[/]");
        AnsiConsole.MarkupLine("");
        if (UserService.LoggedUser.PenaltyFee > 0)
        {
            AnsiConsole.MarkupLine("[yellow]You can't borrow a book until you pay your penalty fee.[/]");
        } else {
            int borrowIdBook = AnsiConsole.Ask<int>("Book ID to borrow:");
            if (bookService.CheckExistingBookDataById(borrowIdBook))
            {
                
                List<Borrowing> borrowingList = HasActiveBorrowings(UserService.LoggedUser.IdNumber);
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
    public void DoBorrowing(int borrowIdBook)
    {
        Book? book = bookService.bookData.BooksList.Find(x => x.IdNumber == borrowIdBook);
        if (book != null)
        {
            if (book.Copies > 0)
            {
                book.Copies -= 1;
                bookService.bookData.SaveBookData();
                Borrowing borrowing = new(UserService.LoggedUser.IdNumber, borrowIdBook);
                if (borrowingData.BorrowingsList.Count == 0)
                {
                    borrowingData.AddBorrowing(borrowing);
                } else {
                    int num = borrowingData.BorrowingsList.Last().IdNumber;
                    num++;
                    borrowing = new(UserService.LoggedUser.IdNumber, borrowIdBook, num);
                    borrowingData.AddBorrowing(borrowing);
                } 
                AnsiConsole.MarkupLine($"[yellow]'{book.Title}' borrowed succesfully![/]");
            } else {
                AnsiConsole.MarkupLine("[yellow]We don't have copies of that book available, sorry :([/]");
            }
        } else {
            AnsiConsole.MarkupLine("[red]ERROR in 'Borrow a book', book = null[/]");
        } 
    }
    public void ReturnBook()
    {
        AnsiConsole.MarkupLine("[green]Returning a book[/]");
        AnsiConsole.MarkupLine("");
        List<Borrowing> borrowingList = HasActiveBorrowings(UserService.LoggedUser.IdNumber);
        if (borrowingList.Count != 0)
        {
            AnsiConsole.MarkupLine("[yellow]You have the following borrowed books:[/]");
            CreateBorrowingsTable(borrowingList, true);
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
                            UserService.LoggedUser.PenaltyFee += (decimal)newPenalty;
                            userService.UpdateLoggedUserPenalty();
                            UpdateBorrowingPenalty(foundBorrowing.IdNumber, (decimal)newPenalty);
                            AnsiConsole.MarkupLine($"[yellow]A penalty fee of[/] [red]{newPenalty}$[/] [yellow]has been added to your account[/]");
                        }
                        book.Copies += 1;
                        borrowingData.SaveBorrowingData();
                        bookService.bookData.SaveBookData();
                        userService.userData.SaveUserData();
                        AnsiConsole.MarkupLine($"[yellow]'{book.Title}' returned succesfully![/]");
                    } else {
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
        List<Borrowing> borrowing_List = HasActiveBorrowings(UserService.LoggedUser.IdNumber);
        if (borrowing_List.Count != 0)
        {
            AnsiConsole.MarkupLine("[yellow]You have the following borrowed books currently:[/]");
            CreateBorrowingsTable(borrowing_List, true);
            AnsiConsole.Write(BookTable);
            AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("<-Back to menu"));
        } else {
            AnsiConsole.MarkupLine("[yellow]You don't have any borrowed books yet.[/]");
            Thread.Sleep(3000);
        }
    }
    public void BorrowedBooksHistory()
    {
        List<Borrowing> borrowing__List = borrowingData.BorrowingsList.FindAll(x => x.UserId == UserService.LoggedUser.IdNumber);
        if (borrowing__List.Count != 0)
        {
            AnsiConsole.MarkupLine("[yellow]This is your borrowed books history:[/]");
            CreateBorrowingsTable(borrowing__List, false);
            AnsiConsole.Write(BookTable);
            AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("<-Back to menu"));
        } else {
            AnsiConsole.MarkupLine("[yellow]You don't have any borrowed books yet.[/]");
            Thread.Sleep(3000);
        }
    }
    public void PayPenaltyFee()
    {
        decimal penalty = UserService.LoggedUser.PenaltyFee;
        if (penalty > 0)
        {
            AnsiConsole.MarkupLine($"You have [yellow]{penalty}$[/] of penalty fee.");
            if (AnsiConsole.Confirm("Do you want to pay it?"))
            {
                AnsiConsole.MarkupLine("[yellow]Congratulations! You have paid your penalty fee. :)[/]");
                UserService.LoggedUser.PenaltyFee = 0;
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
    public List<Borrowing> HasActiveBorrowings(int userId)
    {
        List<Borrowing> list = new();
        foreach (var borrowing in borrowingData.BorrowingsList)
        {
            if (borrowing.UserId == userId && borrowing.Returned == false)
            {
                list.Add(borrowing);
            }
        }
        return list;
    }
    public void CreateBorrowingsTable(List<Borrowing> borrowingList, bool isCurrent)
    {
        List<Book> booksBorrowedlist = new();
        BookTable = new ();
        if (isCurrent){
            BookTable.AddColumns("ID", "Title", "Author", "Borrowing Date", "Borrowing End Date", "Returned Date");
        } else {
            BookTable.AddColumns("ID", "Title", "Author", "Borrowing Date", "Borrowing End Date", "Returned Date", "Penalty Fee");
        }
        foreach (var borrowing in borrowingList)
        {
            foreach (var book in bookService.bookData.BooksList)
            {
                if (book.IdNumber == borrowing.BookId)
                {
                    string borrowingDate = borrowing.BorrowingDate.Value.ToString("dd/MM/yyyy");
                    string dateToReturn = borrowing.DateToReturn.Value.ToString("dd/MM/yyyy");
                    string returnedDate = "Not returned yet.";
                    if (borrowing.ReturnedDate.HasValue){
                        returnedDate = borrowing.ReturnedDate.Value.ToString("dd/MM/yyyy");
                    }
                    if (isCurrent){
                        BookTable.AddRow(book.IdNumber.ToString(), book.Title, book.Author, borrowingDate, dateToReturn, returnedDate);
                    } else {
                        BookTable.AddRow(book.IdNumber.ToString(), book.Title, book.Author, borrowingDate, dateToReturn, returnedDate, borrowing.PenaltyFee.ToString()+" $");
                    }
                    booksBorrowedlist.Add(book);
                }
            }
        }
    }
    public void UpdateBorrowingPenalty(int borrowingId, decimal penalty)
    {
        
        if (borrowingData.BorrowingsList.Find(x => x.IdNumber == borrowingId) != null)
        {
            borrowingData.BorrowingsList.Find(x => x.IdNumber == borrowingId).PenaltyFee = penalty;
        }
    }
}