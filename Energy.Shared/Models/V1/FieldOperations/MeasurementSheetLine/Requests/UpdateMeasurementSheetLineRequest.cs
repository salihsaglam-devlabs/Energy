namespace Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;

/// <summary>MeasurementSheetLine güncelleme isteği.</summary>
public class UpdateMeasurementSheetLineRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>MeasurementSheetId</summary>
    public Guid MeasurementSheetId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }
}
