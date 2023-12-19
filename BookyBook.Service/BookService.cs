using BookyBook.Data;
using BookyBook.Domain;

namespace BookyBook.Service;
public class BookService
{
    public readonly BookData bookData = new();
    public int existingBookIndex;
    public void DonateBook(string title, string author, string genre, int year, int copies, decimal score)
    {
        Book book = new(title, author, genre, year, copies, score);
        if (bookData.BooksList.Count == 0)
        {
            bookData.AddBook(book);
        } else if (CheckExistingBookData(title, author) == false)
        {
            int num = bookData.BooksList.Last().IdNumber;
            num++;
            book = new(title, author, genre, year, copies, score, num);
            bookData.AddBook(book);
        } else {
            bookData.BooksList[existingBookIndex].Copies += book.Copies;
            bookData.SaveBookData();
        }
    }
    public bool CheckExistingBookData(string? title, string? author)
    {
        existingBookIndex = 0;
        foreach (var book in bookData.BooksList)
        {
            if (book.Title == title && book.Author == author)
            {
                return true;
            }
            existingBookIndex++;
        }
        return false;
    }
    public bool CheckExistingBookDataById(int bookId)
    {
        existingBookIndex = 0;
        foreach (var book in bookData.BooksList)
        {
            if (book.IdNumber == bookId)
            {
                return true;
            }
            existingBookIndex++;
        }
        return false;
    }
    public List<Book> SearchBooks(string searchText)
    {
        List<Book> list = new();
        foreach (var book in bookData.BooksList)
        {
            if (book.Title.Contains(searchText) || book.Author.Contains(searchText))
            {
                list.Add(book);
            }
        }
        return list;
    }
    public List<Book> GetBookList()
    {
        bookData.GetRegisteredBooks();
        return bookData.BooksList;
    }
}