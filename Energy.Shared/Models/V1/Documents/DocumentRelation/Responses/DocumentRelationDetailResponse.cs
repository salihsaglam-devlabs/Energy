namespace Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;

/// <summary>DocumentRelation detay görünümü.</summary>
public class DocumentRelationDetailResponse
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

    /// <summary>Documents referansı</summary>
    public Guid DocumentId { get; set; }

    /// <summary>RelatedModule</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>RelatedEntityType</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>RelatedEntityId</summary>
    public Guid RelatedEntityId { get; set; }
}
