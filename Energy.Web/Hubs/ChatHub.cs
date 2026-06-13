using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Energy.Web.Hubs;

/// <summary>
/// Gerçek zamanlı sohbet taşıma katmanı. MVC <c>ChatController</c> mesajları API
/// üzerinden kalıcılaştırır ve ardından bunları <see cref="IHubContext{ChatHub}"/>
/// kullanarak bu hub aracılığıyla ilgili kullanıcılara iletir. Bağlantılar, varsayılan
/// <see cref="IUserIdProvider"/> (NameIdentifier claim) ile kullanıcı bazında gruplanır;
/// böylece kullanıcı, geçerli sayfadan bağımsız olarak her açık sekmede bildirim alır.
/// Mesaj iletiminin yanı sıra hub, çevrimiçi/çevrimdışı durumunu izler ve konuşma
/// tarafları arasında yazıyor göstergelerini aktarır.
/// </summary>
[Authorize]
public sealed class ChatHub : Hub
{
    /// <summary>Sunucu → istemci olay adları.</summary>
    public const string ReceiveMessage = "ReceiveMessage";
    /// <summary>Okunmamış mesaj sayısı değişti olayı.</summary>
    public const string UnreadCountChanged = "UnreadCountChanged";
    /// <summary>Bir kullanıcının çevrimiçi durumu değişti olayı.</summary>
    public const string PresenceChanged = "PresenceChanged";
    /// <summary>Tüm çevrimiçi kullanıcı listesinin anlık görüntüsü olayı.</summary>
    public const string PresenceSnapshot = "PresenceSnapshot";
    /// <summary>Yazıyor göstergesi değişti olayı.</summary>
    public const string TypingChanged = "TypingChanged";

    /// <summary>Bir kullanıcı bir gruba davet edildi (davet edilene iletilir).</summary>
    public const string GroupInvite = "GroupInvite";

    /// <summary>Bir grubun üyeliği/durumu değişti (üyelere iletilir).</summary>
    public const string GroupChanged = "GroupChanged";

    /// <summary>Bir grup silindi (eski üyelerine iletilir).</summary>
    public const string GroupDeleted = "GroupDeleted";

    /// <summary>Bir mesaj herkes için silindi.</summary>
    public const string MessageDeleted = "MessageDeleted";

    /// <summary>Bir mesajın tepkileri değişti.</summary>
    public const string MessageReacted = "MessageReacted";

    /// <summary>Karşı taraf, geçerli kullanıcının mesajlarını okudu (okundu bilgisi).</summary>
    public const string MessagesRead = "MessagesRead";

    // Sesli arama (WebRTC) sinyalleşme olayları (sunucu → istemci).
    /// <summary>Gelen arama teklifi (offer) olayı.</summary>
    public const string CallOffer = "CallOffer";
    /// <summary>Arama yanıtlandı (answer) olayı.</summary>
    public const string CallAnswered = "CallAnswered";
    /// <summary>WebRTC ICE adayı (candidate) olayı.</summary>
    public const string CallIce = "CallIce";
    /// <summary>Arama sonlandı olayı.</summary>
    public const string CallEnded = "CallEnded";

    private readonly IChatPresenceTracker _presence;

    /// <summary>Çevrimiçi durum izleyicisini enjekte eder.</summary>
    public ChatHub(IChatPresenceTracker presence)
    {
        _presence = presence;
    }

    /// <summary>Bağlantı kurulduğunda kullanıcıyı çevrimiçi işaretler ve durumu yayınlar.</summary>
    public override async Task OnConnectedAsync()
    {
        var userId = CurrentUserId;
        if (userId is { } id)
        {
            var becameOnline = _presence.Add(id, Context.ConnectionId);

            // Yeni bağlanan istemciye tüm çevrimiçi listesini ver; böylece durum
            // etiketlerini hemen çizebilir.
            await Clients.Caller.SendAsync(
                PresenceSnapshot,
                _presence.OnlineUsers.Select(u => u.ToString()).ToArray());

            if (becameOnline)
            {
                await Clients.Others.SendAsync(
                    PresenceChanged,
                    new { userId = id.ToString(), isOnline = true });
            }
        }

        await base.OnConnectedAsync();
    }

    /// <summary>Bağlantı koptuğunda kullanıcıyı (son bağlantıysa) çevrimdışı işaretler ve yayınlar.</summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = CurrentUserId;
        if (userId is { } id)
        {
            var becameOffline = _presence.Remove(id, Context.ConnectionId);
            if (becameOffline)
            {
                await Clients.Others.SendAsync(
                    PresenceChanged,
                    new { userId = id.ToString(), isOnline = false });
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>Geçerli kullanıcıdan tek bir tarafa yazıyor göstergesini iletir.</summary>
    public Task Typing(string recipientId, bool isTyping)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(recipientId))
        {
            return Task.CompletedTask;
        }

        return Clients.User(recipientId).SendAsync(
            TypingChanged,
            new { fromUserId = userId.Value.ToString(), isTyping });
    }

    /// <summary>
    /// Bir istemcinin geçerli çevrimiçi listesini yeniden istemesini sağlar — sonraki
    /// bağlan/bağlantı kes olayını beklemeden durumun her zaman güncel olması için
    /// (yeniden) bağlanmanın hemen ardından kullanılır.
    /// </summary>
    public Task RequestPresence()
        => Clients.Caller.SendAsync(
            PresenceSnapshot,
            _presence.OnlineUsers.Select(u => u.ToString()).ToArray());

    // ----- Sesli arama (WebRTC) sinyalleşmesi. Her metot, SDP/ICE bilgisini hedef
    // kullanıcıya aktarır ve yükü, arayanın kimliğiyle etiketler. -------------

    /// <summary>Hedef kullanıcıya bir arama teklifi (offer) gönderir.</summary>
    public Task CallUser(string targetUserId, string callerName, object offer)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(targetUserId)) { return Task.CompletedTask; }
        return Clients.User(targetUserId).SendAsync(CallOffer,
            new { fromUserId = userId.Value.ToString(), callerName, offer });
    }

    /// <summary>Hedef kullanıcıya bir arama yanıtı (answer) gönderir.</summary>
    public Task AnswerCall(string targetUserId, object answer)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(targetUserId)) { return Task.CompletedTask; }
        return Clients.User(targetUserId).SendAsync(CallAnswered,
            new { fromUserId = userId.Value.ToString(), answer });
    }

    /// <summary>Hedef kullanıcıya bir WebRTC ICE adayı gönderir.</summary>
    public Task SendIce(string targetUserId, object candidate)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(targetUserId)) { return Task.CompletedTask; }
        return Clients.User(targetUserId).SendAsync(CallIce,
            new { fromUserId = userId.Value.ToString(), candidate });
    }

    /// <summary>Hedef kullanıcıya aramanın sonlandığını iletir.</summary>
    public Task EndCall(string targetUserId)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(targetUserId)) { return Task.CompletedTask; }
        return Clients.User(targetUserId).SendAsync(CallEnded,
            new { fromUserId = userId.Value.ToString() });
    }

    /// <summary>Bağlam kullanıcı kimliğini Guid olarak çözer (ayrıştırılamazsa null).</summary>
    private Guid? CurrentUserId
        => Guid.TryParse(Context.UserIdentifier, out var id) ? id : null;
}

