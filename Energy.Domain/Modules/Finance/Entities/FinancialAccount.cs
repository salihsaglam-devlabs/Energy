using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

/// <summary>Finans hesabı (kasa, banka vb.).</summary>
public class FinancialAccount : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Cash, Bank, Other.</summary>
    public string AccountType { get; set; } = "Bank";
    public Guid? CurrencyId { get; set; }
    public bool IsActive { get; set; } = true;
}
