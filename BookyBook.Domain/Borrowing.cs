namespace BookyBook.Domain;
public class Borrowing
{
    public int? UserId { get; set; }
    public int? BookId { get; set; }
    public DateTime? BorrowingDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    //public bool Renewed { get; set; } = false;
    public bool Returned { get; set; } = false;
    private int? IdNumber { get; set; }
    private static int IdNumberSeed = 1;

    public Borrowing(){}
    public Borrowing(int userId, int bookId){
        this.UserId = userId;
        this.BookId = bookId;
        this.BorrowingDate = DateTime.Now;
        this.IdNumber = IdNumberSeed;
        IdNumberSeed++;
    }
}
