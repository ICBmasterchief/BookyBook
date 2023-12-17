namespace BookyBook.Domain;
public class Book
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Genre { get; set; }
    public int? Year { get; set; }
    public int? Copies { get; set; }
    public decimal? Score { get; set; }
    public int? IdNumber { get; set; }
    private static int IdNumberSeed = 10001;

    public Book(){}
    public Book(string title, string author, string genre, int year, int copies, decimal score){
        this.Title = title;
        this.Author = author;
        this.Genre = genre;
        this.Year = year;
        this.Copies = copies;
        this.Score = score;
        this.IdNumber = IdNumberSeed;
    }
    public Book(string title, string author, string genre, int year, int copies, decimal score, int? idNumber){
        this.Title = title;
        this.Author = author;
        this.Genre = genre;
        this.Year = year;
        this.Copies = copies;
        this.Score = score;
        this.IdNumber = idNumber;
    }
}
