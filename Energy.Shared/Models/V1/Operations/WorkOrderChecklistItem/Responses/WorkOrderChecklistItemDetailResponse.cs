namespace Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;

/// <summary>WorkOrderChecklistItem detay görünümü.</summary>
public class WorkOrderChecklistItemDetailResponse
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

    /// <summary>WorkOrderChecklistId</summary>
    public Guid WorkOrderChecklistId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>IsCompleted</summary>
    public bool IsCompleted { get; set; }
}
