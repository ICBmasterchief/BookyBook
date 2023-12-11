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

    public User(){}
    public User(string name, string email, string password){
        this.Name = name;
        this.Email = email;
        this.Password = password;
        this.RegistrationDate = DateTime.Now;
        this.PenaltyFee = 0;
        this.IdNumber = IdNumberSeed;
        IdNumberSeed++;
    }
}
