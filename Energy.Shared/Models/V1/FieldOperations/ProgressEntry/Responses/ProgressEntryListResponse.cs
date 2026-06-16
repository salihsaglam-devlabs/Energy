namespace Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;

/// <summary>ProgressEntry liste satırı.</summary>
public class ProgressEntryListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ProjectId</summary>
    public Guid ProjectId { get; set; }

    /// <summary>ProjectPhaseId</summary>
    public Guid? ProjectPhaseId { get; set; }

    /// <summary>EntryDate</summary>
    public DateTime EntryDate { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>Percentage</summary>
    public decimal Percentage { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
