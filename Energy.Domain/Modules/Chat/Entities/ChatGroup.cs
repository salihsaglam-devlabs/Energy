using Energy.Domain.Common;

namespace Energy.Domain.Modules.Chat;

/// <summary>
/// Sohbet grupları
/// </summary>
public class ChatGroup : AuditableEntity
{
    /// <summary>Users referansı</summary>
    public Guid OwnerId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;
}
