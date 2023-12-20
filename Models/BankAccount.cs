namespace FinistBusinessLogic.Models;

public partial class BankAccount
{
    public string AccountNumber { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public string? ClientPhoneNumber { get; set; }

    public virtual Client? ClientPhoneNumberNavigation { get; set; }
}
