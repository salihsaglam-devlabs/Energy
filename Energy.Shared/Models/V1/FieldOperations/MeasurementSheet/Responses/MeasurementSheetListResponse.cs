namespace Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;

/// <summary>MeasurementSheet liste satırı.</summary>
public class MeasurementSheetListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ProjectId</summary>
    public Guid ProjectId { get; set; }

    /// <summary>ContractId</summary>
    public Guid? ContractId { get; set; }

    /// <summary>SheetNo</summary>
    public string SheetNo { get; set; } = string.Empty;

    /// <summary>SheetDate</summary>
    public DateTime SheetDate { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
