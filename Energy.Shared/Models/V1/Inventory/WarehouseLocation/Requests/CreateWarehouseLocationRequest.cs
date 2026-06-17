namespace Energy.Shared.Models.V1.Inventory.WarehouseLocation.Requests;

/// <summary>WarehouseLocation oluşturma isteği.</summary>
public class CreateWarehouseLocationRequest
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
