using Energy.Domain.Common;

namespace Energy.Domain.Documents;

/// <summary>Belge erişim yetkisi (kullanıcı veya rol bazlı).</summary>
public class DocumentPermission : AuditableEntity
{
    public Guid DocumentId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? RoleId { get; set; }
    /// <summary>Read, Write, Manage.</summary>
    public string AccessType { get; set; } = "Read";
}
