using BookyBook.Domain;
using System.Text.Json;

namespace BookyBook.Data;
public class BookData
{
    public List<Book>? BooksList = new();
    private readonly string BookJsonPath = @"..\BookyBook.Data\Data.Books.json";
    public void AddBook(Book book)
    {
        BooksList.Add(book);
        SaveBookData();
    }
    public void GetRegisteredBooks()
    {
        try
        {
        string JsonBooks = File.ReadAllText(BookJsonPath);
        BooksList =  JsonSerializer.Deserialize<List<Book>>(JsonBooks);
        } catch (System.Exception)
        {
            Console.WriteLine("ERROR TRYING ACCESS DATA");
            //throw;
        }
    }
    public void SaveBookData()
    {
        string JsonBooks = JsonSerializer.Serialize(BooksList, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(BookJsonPath, JsonBooks);
    }
}
