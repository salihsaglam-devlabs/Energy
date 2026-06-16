using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>
/// Metraj başlıkları
/// </summary>
public class MeasurementSheet : AuditableEntity
{
    /// <summary>ProjectId</summary>
    public Guid ProjectId { get; set; }

    /// <summary>ContractId</summary>
    public Guid? ContractId { get; set; }

    /// <summary>SheetNo</summary>
    public string SheetNo { get; set; } = string.Empty;

    /// <summary>SheetDate</summary>
    public DateTime SheetDate { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;
}
