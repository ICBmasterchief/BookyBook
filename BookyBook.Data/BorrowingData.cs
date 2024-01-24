using BookyBook.Domain;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace BookyBook.Data;
public class BorrowingData
{
    public List<Borrowing>? BorrowingsList = new();
    private readonly string BorrowingJsonPath;
    public BorrowingData()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            BorrowingJsonPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "BookyBook.Data", "Data.Borrowings.json");
        } else {
            BorrowingJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "BookyBook.Data", "Data.Borrowings.json");
        }
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