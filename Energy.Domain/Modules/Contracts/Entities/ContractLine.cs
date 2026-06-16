using Energy.Domain.Common;

namespace Energy.Domain.Modules.Contracts;

/// <summary>
/// Sözleşme kalemleri
/// </summary>
public class ContractLine : AuditableEntity
{
    /// <summary>ContractId</summary>
    public Guid ContractId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }
}
