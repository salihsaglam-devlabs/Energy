using Energy.Domain.Common;

namespace Energy.Domain.Chat;

/// <summary>
/// Bir <see cref="ChatGroup"/> ile bir kullanıcı arasındaki bağlantı satırı.
/// Davet durumunu taşır; böylece kullanıcı yalnızca daveti kabul ettiğinde gruba
/// dahil edilir.
/// </summary>
public class ChatGroupMember : AuditableEntity
{
    /// <summary>Üyeliğin ait olduğu grubun kimliği.</summary>
    public Guid GroupId { get; set; }

    /// <summary>Üye olan kullanıcının kimliği.</summary>
    public Guid UserId { get; set; }

    /// <summary>Üyelik durumu (beklemede / kabul / red).</summary>
    public ChatGroupMemberStatus Status { get; set; } = ChatGroupMemberStatus.Pending;

    /// <summary>Grup sahibi için true (grubu oluşturan kişi).</summary>
    public bool IsOwner { get; set; }

    /// <summary>
    /// Bu üye bir grup yöneticisiyse true. Yöneticiler (ve her zaman örtük olarak
    /// yönetici sayılan sahip) üye ekleyip çıkarabilir, başka üyeleri yönetici
    /// yapabilir veya yöneticilikten alabilir. Sahibin yöneticiliği alınamaz ve
    /// sahip gruptan çıkarılamaz.
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>Daveti gönderen kullanıcının kimliği (sahibin kendi satırında null).</summary>
    public Guid? InvitedById { get; set; }
}
