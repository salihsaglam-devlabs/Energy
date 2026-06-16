using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;

/// <summary>MeasurementSheet detay görünümü.</summary>
public class MeasurementSheetDetailResponse
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

    /// <summary>ContractId</summary>
    public Guid? ContractId { get; set; }

    /// <summary>SheetNo</summary>
    public string SheetNo { get; set; } = string.Empty;

    /// <summary>SheetDate</summary>
    public DateTime SheetDate { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }
}
