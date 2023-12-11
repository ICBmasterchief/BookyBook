namespace BookyBook.Domain;
public class User
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    private int IdNumber { get; set; }
    public DateTime RegistrationDate { get; set; }
    public decimal PenaltyFee { get; set; }
    private static int IdNumberSeed = 1111;
}
