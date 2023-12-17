using BookyBook.Data;
using BookyBook.Domain;

namespace BookyBook.Service;
public class BookService
{
    public readonly BookData bookData = new();
    public void DonateNewBook(string title, string author, string genre, int year, int copies, decimal score)
    {
        if (bookData.BooksList.Count == 0)
        {
            Book book = new(title, author, genre, year, score);
            bookData.AddBook(book);
        } else if (CheckExistingBookData(title, author) == false)
            {
                    int? num = bookData.BooksList.Last().IdNumber;
                    num++;
                    Book book = new(title, author, genre, year, score, num);
                    bookData.AddBook(book);
            }
    }
    public bool CheckExistingBookData(string? title, string? author)
    {
        foreach (var book in bookData.BooksList)
        {
            if (book.Title == title && book.Author == author)
            {
                return true;
            }
        }
        return false;
    }
}