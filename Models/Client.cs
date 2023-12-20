namespace FinistBusinessLogic.Models;

public partial class Client
{
    public string PhoneNumber { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
