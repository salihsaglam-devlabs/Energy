using Energy.Domain.Common;

namespace Energy.Domain.Modules.ProgressPayments;

/// <summary>Hakediş satırı.</summary>
public class ProgressPaymentLine : AuditableEntity
{
    public Guid ProgressPaymentId { get; set; }
    public Guid? ContractLineId { get; set; }
    public Guid? MeasurementSheetLineId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
}
