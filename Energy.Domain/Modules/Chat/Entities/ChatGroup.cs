using Energy.Domain.Common;

namespace Energy.Domain.Modules.Chat;

/// <summary>
/// Adı olan bir sohbet grubu. Sahibi grubu oluşturur ve kullanıcıları davet eder;
/// davet edilen kullanıcı yalnızca daveti kabul ettiğinde aktif katılımcı olur
/// (bkz. <see cref="ChatGroupMember"/>).
/// </summary>
public class ChatGroup : AuditableEntity
{
    /// <summary>Grup adı.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Grubu oluşturan ve sahibi olan kullanıcının kimliği.</summary>
    public Guid OwnerId { get; set; }
}
