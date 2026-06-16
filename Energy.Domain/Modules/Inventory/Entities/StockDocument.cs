using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Stok hareket belgeleri
/// </summary>
public class StockDocument : AuditableEntity
{
    /// <summary>Belge türü</summary>
    public Guid DocumentTypeId { get; set; }

    /// <summary>Kaynak depo</summary>
    public Guid? SourceWarehouseId { get; set; }

    /// <summary>Hedef depo</summary>
    public Guid? TargetWarehouseId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Belge durumu</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Belge numarası</summary>
    public string DocumentNo { get; set; } = string.Empty;

    /// <summary>DocumentDate</summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
