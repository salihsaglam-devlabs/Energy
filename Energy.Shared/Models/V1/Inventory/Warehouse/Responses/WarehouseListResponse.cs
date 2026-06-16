namespace Energy.Shared.Models.V1.Inventory.Warehouse.Responses;

/// <summary>Warehouse liste satırı.</summary>
public class WarehouseListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Şirket</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Şube</summary>
    public Guid? BranchId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Depo türü</summary>
    public string WarehouseType { get; set; } = string.Empty;

    /// <summary>Depo kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Depo adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
