namespace Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;

/// <summary>ApprovalDefinition detay görünümü.</summary>
public class ApprovalDefinitionDetailResponse
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

    /// <summary>Akış kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Akış adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>İlgili modül</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>İlgili nesne türü</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>Aktiflik</summary>
    public bool IsActive { get; set; }
}
