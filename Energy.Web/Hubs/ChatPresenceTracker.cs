namespace Energy.Web.Hubs;

/// <summary>
/// Tracks which users currently have at least one live SignalR connection so the
/// UI can show WhatsApp-style online/offline presence. A user can be connected
/// from several tabs/devices at once, so connections are reference-counted per
/// user: presence flips to "online" on the first connection and back to
/// "offline" only when the last one drops.
/// </summary>
public interface IChatPresenceTracker
{
    /// <summary>Registers a connection. Returns <c>true</c> when the user just came online.</summary>
    bool Add(Guid userId, string connectionId);

    /// <summary>Removes a connection. Returns <c>true</c> when the user just went offline.</summary>
    bool Remove(Guid userId, string connectionId);

    /// <summary>Snapshot of all users that are currently online.</summary>
    IReadOnlyCollection<Guid> OnlineUsers { get; }

    bool IsOnline(Guid userId);
}

public sealed class ChatPresenceTracker : IChatPresenceTracker
{
    private readonly object _gate = new();
    private readonly Dictionary<Guid, HashSet<string>> _connections = new();

    public bool Add(Guid userId, string connectionId)
    {
        lock (_gate)
        {
            if (!_connections.TryGetValue(userId, out var set))
            {
                _connections[userId] = new HashSet<string>(StringComparer.Ordinal) { connectionId };
                return true; // offline -> online
            }
            set.Add(connectionId);
            return false;
        }
    }

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
                    return true; // online -> offline
                }
            }
            return false;
        }
    }

    public IReadOnlyCollection<Guid> OnlineUsers
    {
        get { lock (_gate) { return _connections.Keys.ToArray(); } }
    }

    public bool IsOnline(Guid userId)
    {
        lock (_gate) { return _connections.ContainsKey(userId); }
    }
}

