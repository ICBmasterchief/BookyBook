namespace BookyBook.Domain;
public class Book
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Genre { get; set; }
    public int Year { get; set; }
    public int Copies { get; set; } = 0;
    public decimal Score { get; set; }
    private int IdNumber { get; set; }
    private static int IdNumberSeed = 10000;

    public Book(){}
    public Book(string title, string author, string genre, int year, decimal score){
        this.Title = title;
        this.Author = author;
        this.Genre = genre;
        this.Year = year;
        this.Copies = this.Copies++;
        this.Score = score;
        this.IdNumber = IdNumberSeed;
        IdNumberSeed++;
    }
}
