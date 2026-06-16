using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

/// <summary>
/// Ödemelerin borçlara dağılımları
/// </summary>
public class PaymentAllocation : AuditableEntity
{
    /// <summary>PaymentId</summary>
    public Guid PaymentId { get; set; }

    /// <summary>PayableId</summary>
    public Guid PayableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }
}
