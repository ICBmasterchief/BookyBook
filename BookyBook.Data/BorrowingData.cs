using BookyBook.Domain;
using System.Text.Json;

namespace BookyBook.Data;
public class BorrowingData
{
    public List<Borrowing>? BorrowingsList = new();
    private readonly string BorrowingJsonPath = @"..\BookyBook.Data\Data.Borrowings.json";
    public BorrowingData()
    {
        GetRegisteredBorrowings();
    }
    public void AddBorrowing(Borrowing borrowing)
    {
        BorrowingsList.Add(borrowing);
        SaveBorrowingData();
    }
    public void GetRegisteredBorrowings()
    {
        try
        {
        string JsonBorrowings = File.ReadAllText(BorrowingJsonPath);
        BorrowingsList =  JsonSerializer.Deserialize<List<Borrowing>>(JsonBorrowings);
        } catch (System.Exception)
        {
            Console.WriteLine("ERROR TRYING ACCESS DATA");
        }
    }
    public void SaveBorrowingData()
    {
        string JsonBorrowings = JsonSerializer.Serialize(BorrowingsList, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(BorrowingJsonPath, JsonBorrowings);
    }
}