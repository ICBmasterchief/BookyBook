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
        } else {
            int num = borrowingData.BorrowingsList.Last().IdNumber;
            num++;
            borrowing = new(userId, bookId, num);
            borrowingData.AddBorrowing(borrowing);
        } 
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
    public void UpdateBorrowingPenalty(int borrowingId, decimal penalty)
    {
        
        if (borrowingData.BorrowingsList.Find(x => x.IdNumber == borrowingId) != null)
        {
            borrowingData.BorrowingsList.Find(x => x.IdNumber == borrowingId).PenaltyFee = penalty;
        }
    }
}