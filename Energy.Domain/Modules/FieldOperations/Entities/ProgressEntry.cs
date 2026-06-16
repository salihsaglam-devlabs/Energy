using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>
/// Proje ilerleme kayıtları
/// </summary>
public class ProgressEntry : AuditableEntity
{
    /// <summary>ProjectId</summary>
    public Guid ProjectId { get; set; }

    /// <summary>ProjectPhaseId</summary>
    public Guid? ProjectPhaseId { get; set; }

    /// <summary>EntryDate</summary>
    public DateTime EntryDate { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>Percentage</summary>
    public decimal Percentage { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
