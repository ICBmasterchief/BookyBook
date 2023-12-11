namespace BookyBook.Domain;
public class Borrowing
{
    public User? User { get; set; }
    public Book? Book { get; set; }
    public DateTime? BorrowingDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool Renewed { get; set; } = false;
    public bool Returned { get; set; } = false;
    private int IdNumber { get; set; }
    private static int IdNumberSeed = 1;

    public Borrowing(){}
    public Borrowing(User user, Book book){
        this.User = user;
        this.Book = book;
        this.BorrowingDate = DateTime.Now;
        this.IdNumber = IdNumberSeed;
        IdNumberSeed++;
    }
}
