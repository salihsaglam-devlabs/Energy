using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;

namespace Energy.Application.Chat.Services;

/// <summary>
/// Kullanıcılar arası birebir (1-1) ve grup mesajlaşması. Her mesaj kalıcı olarak
/// saklanır; gerçek zamanlı iletim, burada döndürülen değerlerin üzerine Web
/// katmanındaki SignalR hub'ı tarafından eklenir.
/// </summary>
public interface IChatService
{
    /// <summary>Geçerli kullanıcının her kişiden okunmamış mesaj sayısıyla birlikte diğer tüm aktif kullanıcıları.</summary>
    Task<IReadOnlyList<ChatContactResponse>> GetContactsAsync(Guid currentUserId, CancellationToken cancellationToken = default);

    /// <summary>Geçerli kullanıcı ile bir kişi arasındaki sıralı mesaj geçmişi.</summary>
    Task<IReadOnlyList<ChatMessageResponse>> GetConversationAsync(Guid currentUserId, Guid peerId, CancellationToken cancellationToken = default);

    /// <summary>Geçerli kullanıcıdan bir mesaj saklar ve kaydedilen satırı döndürür.</summary>
    Task<ChatMessageResponse> SendAsync(Guid senderId, SendChatMessageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bir mesajı herkes için yumuşak siler (yalnızca gönderen silebilir). Silinen
    /// mesaj projeksiyonunu (gerçek zamanlı dağıtım için) döndürür; izin yoksa/bulunamazsa null.
    /// </summary>
    Task<ChatMessageResponse?> DeleteMessageAsync(Guid currentUserId, Guid messageId, CancellationToken cancellationToken = default);

    /// <summary>Mevcut bir mesajın içeriğini yeni bir hedefe iletir. Yeni mesajı döndürür.</summary>
    Task<ChatMessageResponse?> ForwardAsync(Guid currentUserId, ForwardChatMessageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Geçerli kullanıcının bir mesaja verdiği emoji tepkisini aç/kapa yapar (aynı emoji
    /// kaldırır, farklı emoji değiştirir). Güncellenen mesaj projeksiyonunu veya null döndürür.
    /// </summary>
    Task<ChatMessageResponse?> ToggleReactionAsync(Guid currentUserId, Guid messageId, string emoji, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bir mesajın ikili (binary) ekini döndürür; yalnızca geçerli kullanıcı o mesajın
    /// katılımcısıysa (gönderen veya alıcı). Aksi halde <c>null</c> döner.
    /// </summary>
    Task<ChatAttachmentResponse?> GetAttachmentAsync(Guid currentUserId, Guid messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sohbet avatarlarını çizmek için bir kullanıcının profil resmini döndürür. Sohbet
    /// kapsamlıdır (her katılımcı bir başkasının avatarını görebilir); bu nedenle Users
    /// uç noktalarının istediği kullanıcı yönetimi/profil yetkilerini gerektirmez.
    /// </summary>
    Task<ChatAttachmentResponse?> GetUserAvatarAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary><paramref name="peerId"/> kullanıcısından geçerli kullanıcıya gelen tüm mesajları okundu işaretler.</summary>
    Task<int> MarkReadAsync(Guid currentUserId, Guid peerId, CancellationToken cancellationToken = default);

    /// <summary>Geçerli kullanıcıya gönderilen toplam okunmamış mesaj sayısı (genel zili besler).</summary>
    Task<int> GetUnreadCountAsync(Guid currentUserId, CancellationToken cancellationToken = default);

    // ----- Gruplar ----------------------------------------------------------

    /// <summary>Geçerli kullanıcının aktif olarak üye olduğu gruplar (sahip veya kabul etmiş üye).</summary>
    Task<IReadOnlyList<ChatGroupResponse>> GetGroupsAsync(Guid currentUserId, CancellationToken cancellationToken = default);

    /// <summary>Geçerli kullanıcıya gönderilmiş bekleyen grup davetleri.</summary>
    Task<IReadOnlyList<ChatGroupInviteResponse>> GetGroupInvitesAsync(Guid currentUserId, CancellationToken cancellationToken = default);

    /// <summary>Geçerli kullanıcının sahip olduğu bir grup oluşturur ve istenen üyeleri davet eder.</summary>
    Task<ChatGroupResponse> CreateGroupAsync(Guid ownerId, CreateChatGroupRequest request, CancellationToken cancellationToken = default);

    /// <summary>Geçerli kullanıcının üye olduğu bir gruba kullanıcı davet eder. Davet edilen kullanıcı kimliklerini döndürür.</summary>
    Task<IReadOnlyList<Guid>> InviteToGroupAsync(Guid currentUserId, Guid groupId, InviteToGroupRequest request, CancellationToken cancellationToken = default);

    /// <summary>Geçerli kullanıcının bir gruba olan bekleyen davetini kabul eder veya reddeder.</summary>
    Task<bool> RespondInviteAsync(Guid currentUserId, Guid groupId, bool accept, CancellationToken cancellationToken = default);

    /// <summary>Bir grubun üyeleri (geçerli kullanıcı kabul etmiş üye olmalıdır).</summary>
    Task<IReadOnlyList<ChatGroupMemberResponse>> GetGroupMembersAsync(Guid currentUserId, Guid groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bir grubu siler (yalnızca sahip veya yönetici). Grubu, üyeliklerini ve
    /// mesajlarını yumuşak siler. İzin yoksa/bulunamazsa false döner.
    /// </summary>
    Task<bool> DeleteGroupAsync(Guid currentUserId, Guid groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bir üyeyi gruptan çıkarır (yalnızca sahip veya yönetici). Sahip asla
    /// çıkarılamaz. İzin yoksa/bulunamazsa false döner.
    /// </summary>
    Task<bool> RemoveMemberAsync(Guid currentUserId, Guid groupId, Guid memberUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bir üyeyi grup yöneticisi yapar veya yöneticilikten alır (yalnızca sahip veya
    /// yönetici). Sahibin yönetici durumu değiştirilemez. İzin yoksa/bulunamazsa false döner.
    /// </summary>
    Task<bool> SetMemberAdminAsync(Guid currentUserId, Guid groupId, Guid memberUserId, bool isAdmin, CancellationToken cancellationToken = default);

    /// <summary>Gerçek zamanlı dağıtımda kullanılmak üzere bir grubun kabul etmiş üye kimlikleri.</summary>
    Task<IReadOnlyList<Guid>> GetGroupMemberIdsAsync(Guid groupId, CancellationToken cancellationToken = default);

    /// <summary>Bir grubun sıralı mesaj geçmişi (geçerli kullanıcı kabul etmiş üye olmalıdır).</summary>
    Task<IReadOnlyList<ChatMessageResponse>> GetGroupConversationAsync(Guid currentUserId, Guid groupId, CancellationToken cancellationToken = default);
}
