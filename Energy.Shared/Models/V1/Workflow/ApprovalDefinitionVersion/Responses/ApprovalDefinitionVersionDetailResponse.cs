namespace Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;

/// <summary>ApprovalDefinitionVersion detay görünümü.</summary>
public class ApprovalDefinitionVersionDetailResponse
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

    /// <summary>Onay tanımı</summary>
    public Guid ApprovalDefinitionId { get; set; }

    /// <summary>Versiyon</summary>
    public int VersionNo { get; set; }

    /// <summary>Başlangıç</summary>
    public DateTime EffectiveFrom { get; set; }

    /// <summary>Bitiş</summary>
    public DateTime? EffectiveTo { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
