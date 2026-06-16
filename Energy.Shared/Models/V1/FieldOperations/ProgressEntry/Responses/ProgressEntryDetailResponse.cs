namespace Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;

/// <summary>ProgressEntry detay görünümü.</summary>
public class ProgressEntryDetailResponse
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
}
