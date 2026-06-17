using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Documents;

/// <summary>Belge kaydı. Tüm modüllere bağlanabilir ama hiçbirine zorunlu bağımlı değildir.</summary>
public class Document : AuditableEntity
{
    public Guid? DocumentFolderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
    public int CurrentVersionNo { get; set; }
}
