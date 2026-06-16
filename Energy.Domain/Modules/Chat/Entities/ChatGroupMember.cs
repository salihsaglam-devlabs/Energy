using Energy.Domain.Common;

namespace Energy.Domain.Modules.Chat;

/// <summary>
/// Grup üyeleri
/// </summary>
public class ChatGroupMember : AuditableEntity
{
    /// <summary>ChatGroups referansı</summary>
    public Guid GroupId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }
}
