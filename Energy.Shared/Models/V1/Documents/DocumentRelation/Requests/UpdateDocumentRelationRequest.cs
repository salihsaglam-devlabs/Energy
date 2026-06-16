namespace Energy.Shared.Models.V1.Documents.DocumentRelation.Requests;

/// <summary>DocumentRelation güncelleme isteği.</summary>
public class UpdateDocumentRelationRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Documents referansı</summary>
    public Guid DocumentId { get; set; }

    /// <summary>RelatedModule</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>RelatedEntityType</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>RelatedEntityId</summary>
    public Guid RelatedEntityId { get; set; }
}
