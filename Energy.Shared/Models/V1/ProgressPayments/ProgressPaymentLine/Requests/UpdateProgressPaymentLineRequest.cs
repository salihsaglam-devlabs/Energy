namespace Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Requests;

/// <summary>ProgressPaymentLine güncelleme isteği.</summary>
public class UpdateProgressPaymentLineRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
}
