namespace Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;

/// <summary>ProgressPaymentLine liste satırı.</summary>
public class ProgressPaymentLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
