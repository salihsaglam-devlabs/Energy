using Energy.Domain.Common;

namespace Energy.Domain.Inventory;

/// <summary>Depo. Şirkete bağlı; şube ve proje opsiyonel.</summary>
public class Warehouse : AuditableEntity
{
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? ProjectId { get; set; }
    public WarehouseType WarehouseType { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

/// <summary>Depo içi raf/alan hiyerarşisi.</summary>
public class WarehouseLocation : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public Guid? ParentLocationId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

/// <summary>Stok belge türü (giriş/çıkış/transfer/düzeltme).</summary>
public class StockDocumentType : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>In, Out, Transfer, Adjustment.</summary>
    public string Direction { get; set; } = "In";
    public bool IsActive { get; set; } = true;
}

/// <summary>Stok hareket belgesi.</summary>
public class StockDocument : AuditableEntity
{
    public Guid DocumentTypeId { get; set; }
    public Guid? SourceWarehouseId { get; set; }
    public Guid? TargetWarehouseId { get; set; }
    public Guid? ProjectId { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime DocumentDate { get; set; }
    public string? Note { get; set; }
    public Guid? ApprovalRequestId { get; set; }
}

/// <summary>Stok belge satırı.</summary>
public class StockDocumentLine : AuditableEntity
{
    public Guid StockDocumentId { get; set; }
    public Guid MaterialId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public decimal Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? Note { get; set; }
}

/// <summary>Lot ve maliyet katmanı (her giriş ayrı lot).</summary>
public class StockLot : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public Guid MaterialId { get; set; }
    public Guid SourceStockDocumentLineId { get; set; }
    public string LotNo { get; set; } = string.Empty;
    public decimal InitialQuantity { get; set; }
    public decimal RemainingQuantity { get; set; }
    public decimal UnitCost { get; set; }
    public DateTime ReceivedAt { get; set; }
}

/// <summary>Çıkış satırının lotlara FIFO dağılımı.</summary>
public class StockIssueAllocation : AuditableEntity
{
    public Guid StockDocumentLineId { get; set; }
    public Guid StockLotId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
}

/// <summary>Değiştirilemez stok hareketi.</summary>
public class StockTransaction : AuditableEntity
{
    public Guid StockDocumentId { get; set; }
    public Guid StockDocumentLineId { get; set; }
    public Guid? StockLotId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid MaterialId { get; set; }
    /// <summary>İşaretli miktar: giriş (+), çıkış (-).</summary>
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public DateTime TransactionDate { get; set; }
}

/// <summary>Özet stok bakiyesi (hareketlerden yeniden üretilebilir).</summary>
public class StockBalance : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime LastRecalculatedAt { get; set; }
}

/// <summary>Stok rezervasyonu.</summary>
public class StockReservation : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public string? RelatedModule { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public bool IsReleased { get; set; }
}

/// <summary>Depo sayım başlığı.</summary>
public class StockCount : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public string CountNo { get; set; } = string.Empty;
    public DateTime CountDate { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}

/// <summary>Depo sayım satırı.</summary>
public class StockCountLine : AuditableEntity
{
    public Guid StockCountId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal SystemQuantity { get; set; }
    public decimal CountedQuantity { get; set; }
}

/// <summary>Depolar arası transfer başlığı.</summary>
public class WarehouseTransfer : AuditableEntity
{
    public Guid SourceWarehouseId { get; set; }
    public Guid TargetWarehouseId { get; set; }
    public string TransferNo { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}

/// <summary>Depolar arası transfer satırı.</summary>
public class WarehouseTransferLine : AuditableEntity
{
    public Guid WarehouseTransferId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
}

