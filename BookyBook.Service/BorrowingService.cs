using BookyBook.Data;
using BookyBook.Domain;

#pragma warning restore format
namespace BookyBook.Service;
public class BorrowingService
{
    public readonly BorrowingData borrowingData = new();
    public int existingBorrowIndex;
    public void BorrowBook(int userId, int bookId)
    {
        Borrowing borrowing = new(userId, bookId);
        if (borrowingData.BorrowingsList.Count == 0)
        {
            borrowingData.AddBorrowing(borrowing);
        } else if (CheckExistingBorrowingData(userId, bookId) == false)
        {
            int num = borrowingData.BorrowingsList.Last().IdNumber;
            num++;
            borrowing = new(userId, bookId, num);
            borrowingData.AddBorrowing(borrowing);
        }
    }
    public bool CheckExistingBorrowingData(int userId, int bookId)
    {
        existingBorrowIndex = 0;
        foreach (var borrowing in borrowingData.BorrowingsList)
        {
            if (borrowing.UserId == userId && borrowing.BookId == bookId)
            {
                return true;
            }
            existingBorrowIndex++;
        }
        return false;
    }
    public List<Borrowing> HasBorrowings(int userId)
    {
        List<Borrowing> list = new();
        foreach (var borrowing in borrowingData.BorrowingsList)
        {
            if (borrowing.UserId == userId)
            {
                list.Add(borrowing);
            }
        }
        return list;
    }
}