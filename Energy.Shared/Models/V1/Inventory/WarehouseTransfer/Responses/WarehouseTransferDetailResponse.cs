using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;

/// <summary>WarehouseTransfer detay görünümü.</summary>
public class WarehouseTransferDetailResponse
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

    /// <summary>SourceWarehouseId</summary>
    public Guid SourceWarehouseId { get; set; }

    /// <summary>TargetWarehouseId</summary>
    public Guid TargetWarehouseId { get; set; }

    /// <summary>TransferNo</summary>
    public string TransferNo { get; set; } = string.Empty;

    /// <summary>TransferDate</summary>
    public DateTime TransferDate { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }
}
