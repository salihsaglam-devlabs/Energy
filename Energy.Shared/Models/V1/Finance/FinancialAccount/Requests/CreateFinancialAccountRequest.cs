namespace Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;

/// <summary>FinancialAccount oluşturma isteği.</summary>
public class CreateFinancialAccountRequest
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>AccountType</summary>
    public string AccountType { get; set; } = string.Empty;

    /// <summary>CurrencyId</summary>
    public Guid? CurrencyId { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
