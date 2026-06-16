namespace Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;

/// <summary>MeasurementSheetLine liste satırı.</summary>
public class MeasurementSheetLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>MeasurementSheetId</summary>
    public Guid MeasurementSheetId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
