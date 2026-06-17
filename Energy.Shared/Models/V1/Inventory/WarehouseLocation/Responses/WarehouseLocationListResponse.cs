namespace Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;

/// <summary>WarehouseLocation liste satırı.</summary>
public class WarehouseLocationListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Warehouses referansı</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>WarehouseLocations referansı</summary>
    public Guid? ParentLocationId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
