using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Inventory.Warehouse.Requests;

/// <summary>Warehouse oluşturma isteği.</summary>
public class CreateWarehouseRequest
{
    /// <summary>Şirket</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Şube</summary>
    public Guid? BranchId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Depo türü</summary>
    public WarehouseType WarehouseType { get; set; }

    /// <summary>Depo kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Depo adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
