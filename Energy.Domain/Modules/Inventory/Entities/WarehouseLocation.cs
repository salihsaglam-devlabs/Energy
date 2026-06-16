using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Raf, alan ve lokasyon hiyerarşisi
/// </summary>
public class WarehouseLocation : AuditableEntity
{
    /// <summary>Warehouses referansı</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>WarehouseLocations referansı</summary>
    public Guid? ParentLocationId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;
}
