using Energy.Domain.Common;

namespace Energy.Domain.Modules.Documents;

/// <summary>
/// Belge erişim yetkileri
/// </summary>
public class DocumentPermission : AuditableEntity
{
    /// <summary>DocumentId</summary>
    public Guid DocumentId { get; set; }

    /// <summary>UserId</summary>
    public Guid? UserId { get; set; }

    /// <summary>RoleId</summary>
    public Guid? RoleId { get; set; }

    /// <summary>AccessType</summary>
    public string AccessType { get; set; } = string.Empty;
}
