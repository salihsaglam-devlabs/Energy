using Energy.Domain.Common;

namespace Energy.Domain.Modules.Documents;

/// <summary>
/// Belge ve iş nesnesi bağlantıları
/// </summary>
public class DocumentRelation : AuditableEntity
{
    /// <summary>Documents referansı</summary>
    public Guid DocumentId { get; set; }

    /// <summary>RelatedModule</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>RelatedEntityType</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>RelatedEntityId</summary>
    public Guid RelatedEntityId { get; set; }
}
