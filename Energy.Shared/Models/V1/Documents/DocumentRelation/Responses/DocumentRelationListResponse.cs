namespace Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;

/// <summary>DocumentRelation liste satırı.</summary>
public class DocumentRelationListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Documents referansı</summary>
    public Guid DocumentId { get; set; }

    /// <summary>RelatedModule</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>RelatedEntityType</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>RelatedEntityId</summary>
    public Guid RelatedEntityId { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
