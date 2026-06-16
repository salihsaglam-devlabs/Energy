using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Inventory.StockDocument.Responses;

/// <summary>StockDocument liste satırı.</summary>
public class StockDocumentListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
