using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Inventory.StockCount.Responses;

/// <summary>StockCount detay görünümü.</summary>
public class StockCountDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>CountNo</summary>
    public string CountNo { get; set; } = string.Empty;

    /// <summary>CountDate</summary>
    public DateTime CountDate { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }
}
