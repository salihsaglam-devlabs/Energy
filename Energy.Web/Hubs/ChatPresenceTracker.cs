namespace Energy.Web.Hubs;

/// <summary>
/// Hangi kullanıcıların şu anda en az bir canlı SignalR bağlantısına sahip olduğunu izler;
/// böylece arayüz WhatsApp tarzı çevrimiçi/çevrimdışı varlığı (presence) gösterebilir. Bir
/// kullanıcı aynı anda birkaç sekme/cihazdan bağlı olabilir; bu yüzden bağlantılar kullanıcı
/// başına referans sayılır: varlık ilk bağlantıda "çevrimiçi"ye döner ve yalnızca sonuncusu
/// koptuğunda "çevrimdışı"na geri döner.
/// </summary>
public interface IChatPresenceTracker
{
    /// <summary>Bir bağlantıyı kaydeder. Kullanıcı yeni çevrimiçi olduysa <c>true</c> döndürür.</summary>
    bool Add(Guid userId, string connectionId);

    /// <summary>Bir bağlantıyı kaldırır. Kullanıcı yeni çevrimdışı olduysa <c>true</c> döndürür.</summary>
    bool Remove(Guid userId, string connectionId);

    /// <summary>Şu anda çevrimiçi olan tüm kullanıcıların anlık görüntüsü.</summary>
    IReadOnlyCollection<Guid> OnlineUsers { get; }

    /// <summary>Verilen kullanıcının çevrimiçi olup olmadığını döndürür.</summary>
    bool IsOnline(Guid userId);
}

/// <summary>Çevrimiçi kullanıcıları bağlantı referans sayımıyla izleyen iş parçacığı güvenli uygulama.</summary>
public sealed class ChatPresenceTracker : IChatPresenceTracker
{
    private readonly object _gate = new();
    private readonly Dictionary<Guid, HashSet<string>> _connections = new();

    /// <inheritdoc />
    public bool Add(Guid userId, string connectionId)
    {
        lock (_gate)
        {
            if (!_connections.TryGetValue(userId, out var set))
            {
                _connections[userId] = new HashSet<string>(StringComparer.Ordinal) { connectionId };
                return true; // çevrimdışı -> çevrimiçi
            }
            set.Add(connectionId);
            return false;
        }
    }

    /// <inheritdoc />
    public bool Remove(Guid userId, string connectionId)
    {
        lock (_gate)
        {
            if (_connections.TryGetValue(userId, out var set))
            {
                set.Remove(connectionId);
                if (set.Count == 0)
                {
                    _connections.Remove(userId);
                    return true; // çevrimiçi -> çevrimdışı
                }
            }
            return false;
        }
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Guid> OnlineUsers
    {
        get { lock (_gate) { return _connections.Keys.ToArray(); } }
    }

    /// <inheritdoc />
    public bool IsOnline(Guid userId)
    {
        lock (_gate) { return _connections.ContainsKey(userId); }
    }
}

