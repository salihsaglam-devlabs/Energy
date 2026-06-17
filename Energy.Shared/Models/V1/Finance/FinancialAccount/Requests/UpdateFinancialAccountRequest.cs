namespace Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;

/// <summary>FinancialAccount güncelleme isteği.</summary>
public class UpdateFinancialAccountRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
