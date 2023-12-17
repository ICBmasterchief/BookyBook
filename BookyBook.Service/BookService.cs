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
            int? num = bookData.BooksList.Last().IdNumber;
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
    public Book SearchBook(string title, string author)
    {
        if (CheckExistingBookData(title, author))
        {
            return bookData.BooksList[existingBookIndex];
        }
        return new();
    }
}