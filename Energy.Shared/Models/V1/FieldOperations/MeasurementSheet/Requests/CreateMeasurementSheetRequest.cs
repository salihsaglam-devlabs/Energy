using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Requests;

/// <summary>MeasurementSheet oluşturma isteği.</summary>
public class CreateMeasurementSheetRequest
{
    /// <summary>ProjectId</summary>
    public Guid ProjectId { get; set; }

    /// <summary>ContractId</summary>
    public Guid? ContractId { get; set; }

    /// <summary>SheetNo</summary>
    public string SheetNo { get; set; } = string.Empty;

    /// <summary>SheetDate</summary>
    public DateTime SheetDate { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }
}
