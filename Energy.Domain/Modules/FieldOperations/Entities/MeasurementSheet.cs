using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>Metraj başlığı.</summary>
public class MeasurementSheet : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? ContractId { get; set; }
    public string SheetNo { get; set; } = string.Empty;
    public DateTime SheetDate { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}
