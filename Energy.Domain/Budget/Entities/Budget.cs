using Energy.Domain.Common;

namespace Energy.Domain.Budget;

/// <summary>Bütçe başlığı (proje bazlı).</summary>
public class Budget : AuditableEntity
{
    public Guid? ProjectId { get; set; }
    public Guid? CostCenterId { get; set; }
    public Guid CurrencyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public bool IsActive { get; set; } = true;
}
