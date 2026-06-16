using Energy.Domain.Common;

namespace Energy.Domain.Documents;

/// <summary>Belge ↔ iş nesnesi bağlantısı (çok biçimli).</summary>
public class DocumentRelation : AuditableEntity
{
    public Guid DocumentId { get; set; }
    public string RelatedModule { get; set; } = string.Empty;
    public string RelatedEntityType { get; set; } = string.Empty;
    public Guid RelatedEntityId { get; set; }
}
