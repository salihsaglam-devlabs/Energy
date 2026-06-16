namespace Energy.Shared.Models.V1.Inventory.StockLot.Responses;

/// <summary>StockLot detay görünümü.</summary>
public class StockLotDetailResponse
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

    /// <summary>Depo</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>Malzeme</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Kaynak giriş satırı</summary>
    public Guid SourceStockDocumentLineId { get; set; }

    /// <summary>Lot no</summary>
    public string LotNo { get; set; } = string.Empty;

    /// <summary>İlk miktar</summary>
    public decimal InitialQuantity { get; set; }

    /// <summary>Kalan miktar</summary>
    public decimal RemainingQuantity { get; set; }

    /// <summary>Maliyet</summary>
    public decimal UnitCost { get; set; }

    /// <summary>ReceivedAt</summary>
    public DateTime ReceivedAt { get; set; }
}
