namespace Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;

/// <summary>ProgressPaymentLine detay görünümü.</summary>
public class ProgressPaymentLineDetailResponse
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

    /// <summary>ProgressPayments referansı</summary>
    public Guid ProgressPaymentId { get; set; }

    /// <summary>ContractLines referansı</summary>
    public Guid? ContractLineId { get; set; }

    /// <summary>MeasurementSheetLineId</summary>
    public Guid? MeasurementSheetLineId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }
}
