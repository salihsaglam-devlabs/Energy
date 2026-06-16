using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

/// <summary>
/// Tahsilatların alacaklara dağılımları
/// </summary>
public class CollectionAllocation : AuditableEntity
{
    /// <summary>CollectionId</summary>
    public Guid CollectionId { get; set; }

    /// <summary>ReceivableId</summary>
    public Guid ReceivableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }
}
