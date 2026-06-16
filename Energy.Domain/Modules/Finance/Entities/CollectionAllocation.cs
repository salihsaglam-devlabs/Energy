using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

/// <summary>Tahsilatın alacaklara dağılımı.</summary>
public class CollectionAllocation : AuditableEntity
{
    public Guid CollectionId { get; set; }
    public Guid ReceivableId { get; set; }
    public decimal Amount { get; set; }
}
