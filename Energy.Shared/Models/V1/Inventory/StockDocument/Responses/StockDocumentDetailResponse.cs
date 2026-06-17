using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Inventory.StockDocument.Responses;

/// <summary>StockDocument detay görünümü.</summary>
public class StockDocumentDetailResponse
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

    /// <summary>Belge türü</summary>
    public Guid DocumentTypeId { get; set; }

    /// <summary>Kaynak depo</summary>
    public Guid? SourceWarehouseId { get; set; }

    /// <summary>Hedef depo</summary>
    public Guid? TargetWarehouseId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Belge durumu</summary>
    public DocumentStatus Status { get; set; }

    /// <summary>Belge numarası</summary>
    public string DocumentNo { get; set; } = string.Empty;

    /// <summary>DocumentDate</summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
