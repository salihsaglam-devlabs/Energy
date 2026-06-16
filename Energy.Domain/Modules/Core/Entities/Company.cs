using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>Ana organizasyon kökü (şirket).</summary>
public class Company : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Ana para birimi. <see cref="Currency"/> FK.</summary>
    public Guid BaseCurrencyId { get; set; }
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
}
