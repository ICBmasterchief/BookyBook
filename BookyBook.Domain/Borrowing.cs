using System.Data.Common;

namespace BookyBook.Domain;
public class Borrowing
{
    public int? UserId { get; set; }
    public int? BookId { get; set; }
    public DateTime? BorrowingDate { get; set; }
    public DateTime? DateToReturn { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public bool Returned { get; set; } = false;
    public decimal PenaltyFee { get; set; } = 0;
    public int IdNumber { get; set; }
    private static int IdNumberSeed = 1;

    public Borrowing(){}
    public Borrowing(int userId, int bookId){
        this.UserId = userId;
        this.BookId = bookId;
        this.BorrowingDate = DateTime.Today;
        this.DateToReturn = DateTime.Today.AddDays(14);
        this.IdNumber = IdNumberSeed;
    }
    public Borrowing(int userId, int bookId, int idNumber){
        this.UserId = userId;
        this.BookId = bookId;
        this.BorrowingDate = DateTime.Today;
        this.DateToReturn = DateTime.Today.AddDays(14);
        this.IdNumber = idNumber;
    }
}
