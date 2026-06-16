using Energy.Domain.Common;

namespace Energy.Domain.Finance;

/// <summary>Ödemenin borçlara dağılımı.</summary>
public class PaymentAllocation : AuditableEntity
{
    public Guid PaymentId { get; set; }
    public Guid PayableId { get; set; }
    public decimal Amount { get; set; }
}
