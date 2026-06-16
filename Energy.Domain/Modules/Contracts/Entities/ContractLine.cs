using Energy.Domain.Common;

namespace Energy.Domain.Modules.Contracts;

/// <summary>Sözleşme kalemi.</summary>
public class ContractLine : AuditableEntity
{
    public Guid ContractId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
