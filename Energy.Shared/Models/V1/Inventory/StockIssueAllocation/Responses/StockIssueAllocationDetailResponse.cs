namespace Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;

/// <summary>StockIssueAllocation detay görünümü.</summary>
public class StockIssueAllocationDetailResponse
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

    /// <summary>Çıkış satırı</summary>
    public Guid StockDocumentLineId { get; set; }

    /// <summary>Lot</summary>
    public Guid StockLotId { get; set; }

    /// <summary>Dağıtılan miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Maliyet</summary>
    public decimal UnitCost { get; set; }
}
