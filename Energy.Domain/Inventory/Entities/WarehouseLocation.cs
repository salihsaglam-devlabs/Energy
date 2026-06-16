using Energy.Domain.Common;

namespace Energy.Domain.Inventory;

/// <summary>Depo içi raf/alan hiyerarşisi.</summary>
public class WarehouseLocation : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public Guid? ParentLocationId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
