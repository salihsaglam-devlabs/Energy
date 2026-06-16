using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>Proje ilerleme kaydı (miktar ve yüzde bazlı).</summary>
public class ProgressEntry : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? ProjectPhaseId { get; set; }
    public DateTime EntryDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal Percentage { get; set; }
    public string? Note { get; set; }
}
