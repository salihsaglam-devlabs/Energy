using Energy.Application.Chat.Services;
using Energy.Domain.Chat;
using Energy.Domain.Identity;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Chat.Services;

public sealed class ChatService : IChatService
{
    private readonly AppDbContext _db;

    public ChatService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ChatContactResponse>> GetContactsAsync(Guid currentUserId, CancellationToken ct = default)
    {
        var users = await _db.Users.AsNoTracking()
            .Where(u => u.IsActive && u.Id != currentUserId)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.UserName,
                HasProfileImage = u.ProfileImage != null
            })
            .ToListAsync(ct);

        var unreadBySender = await _db.ChatMessages.AsNoTracking()
            .Where(m => m.RecipientId == currentUserId && !m.IsRead)
            .GroupBy(m => m.SenderId)
            .Select(g => new { SenderId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SenderId, x => x.Count, ct);

        var lastByPeer = await _db.ChatMessages.AsNoTracking()
            .Where(m => m.SenderId == currentUserId || m.RecipientId == currentUserId)
            .GroupBy(m => m.SenderId == currentUserId ? m.RecipientId : m.SenderId)
            .Select(g => new { PeerId = g.Key, LastAt = g.Max(m => m.CreatedAt) })
            .ToDictionaryAsync(x => x.PeerId, x => x.LastAt, ct);

        return users
            .Select(u => new ChatContactResponse
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}".Trim(),
                UserName = u.UserName,
                HasProfileImage = u.HasProfileImage,
                UnreadCount = unreadBySender.GetValueOrDefault(u.Id),
                LastMessageAt = lastByPeer.TryGetValue(u.Id, out var at) ? at : null
            })
            .OrderByDescending(c => c.LastMessageAt ?? DateTime.MinValue)
            .ThenBy(c => c.FullName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task<IReadOnlyList<ChatMessageResponse>> GetConversationAsync(Guid currentUserId, Guid peerId, CancellationToken ct = default)
    {
        // Project only metadata (never the attachment bytes) so listing a long
        // conversation stays cheap; the bytes are streamed on demand via GetAttachmentAsync.
        var rows = await _db.ChatMessages.AsNoTracking()
            .Where(m => (m.SenderId == currentUserId && m.RecipientId == peerId)
                     || (m.SenderId == peerId && m.RecipientId == currentUserId))
            .OrderBy(m => m.CreatedAt)
            .Join(_db.Users.AsNoTracking(), m => m.SenderId, u => u.Id,
                (m, u) => new ChatMessageResponse
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    SenderName = ((u.FirstName ?? string.Empty) + " " + (u.LastName ?? string.Empty)).Trim(),
                    SenderHasProfileImage = u.ProfileImage != null,
                    RecipientId = m.RecipientId,
                    Text = m.Text,
                    SentAt = m.CreatedAt,
                    IsRead = m.IsRead,
                    HasAttachment = m.AttachmentData != null,
                    AttachmentFileName = m.AttachmentFileName,
                    AttachmentContentType = m.AttachmentContentType
                })
            .ToListAsync(ct);

        return rows;
    }

    public async Task<ChatMessageResponse> SendAsync(Guid senderId, SendChatMessageRequest request, CancellationToken ct = default)
    {
        byte[]? attachmentData = null;
        string? attachmentName = null;
        string? attachmentContentType = null;
        if (!string.IsNullOrWhiteSpace(request.AttachmentContentBase64))
        {
            attachmentData = Convert.FromBase64String(request.AttachmentContentBase64);
            attachmentName = string.IsNullOrWhiteSpace(request.AttachmentFileName) ? "file" : request.AttachmentFileName.Trim();
            attachmentContentType = string.IsNullOrWhiteSpace(request.AttachmentContentType)
                ? "application/octet-stream"
                : request.AttachmentContentType.Trim();
        }

        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            RecipientId = request.RecipientId,
            Text = (request.Text ?? string.Empty).Trim(),
            IsRead = false,
            AttachmentFileName = attachmentName,
            AttachmentContentType = attachmentContentType,
            AttachmentData = attachmentData
        };
        _db.ChatMessages.Add(message);
        await _db.SaveChangesAsync(ct);

        var sender = await _db.Users.AsNoTracking()
            .Where(u => u.Id == senderId)
            .Select(u => new { Name = $"{u.FirstName} {u.LastName}".Trim(), HasImage = u.ProfileImage != null })
            .FirstOrDefaultAsync(ct);

        return Map(message, sender?.Name ?? string.Empty, sender?.HasImage ?? false);
    }

    public async Task<ChatAttachmentResponse?> GetAttachmentAsync(Guid currentUserId, Guid messageId, CancellationToken ct = default)
    {
        var row = await _db.ChatMessages.AsNoTracking()
            .Where(m => m.Id == messageId
                     && (m.SenderId == currentUserId || m.RecipientId == currentUserId)
                     && m.AttachmentData != null)
            .Select(m => new { m.AttachmentData, m.AttachmentContentType, m.AttachmentFileName })
            .FirstOrDefaultAsync(ct);

        if (row?.AttachmentData is null)
        {
            return null;
        }

        return new ChatAttachmentResponse
        {
            Content = row.AttachmentData,
            ContentType = string.IsNullOrWhiteSpace(row.AttachmentContentType) ? "application/octet-stream" : row.AttachmentContentType,
            FileName = string.IsNullOrWhiteSpace(row.AttachmentFileName) ? "file" : row.AttachmentFileName
        };
    }

    public async Task<ChatAttachmentResponse?> GetUserAvatarAsync(Guid userId, CancellationToken ct = default)
    {
        var row = await _db.Users.AsNoTracking()
            .Where(u => u.Id == userId && u.ProfileImage != null)
            .Select(u => new { u.ProfileImage, u.ProfileImageContentType })
            .FirstOrDefaultAsync(ct);

        if (row?.ProfileImage is null)
        {
            return null;
        }

        return new ChatAttachmentResponse
        {
            Content = row.ProfileImage,
            ContentType = string.IsNullOrWhiteSpace(row.ProfileImageContentType) ? "image/png" : row.ProfileImageContentType,
            FileName = "avatar"
        };
    }

    public async Task<int> MarkReadAsync(Guid currentUserId, Guid peerId, CancellationToken ct = default)
    {
        var unread = await _db.ChatMessages
            .Where(m => m.RecipientId == currentUserId && m.SenderId == peerId && !m.IsRead)
            .ToListAsync(ct);
        if (unread.Count == 0) return 0;

        var now = DateTime.UtcNow;
        foreach (var m in unread)
        {
            m.IsRead = true;
            m.ReadAt = now;
        }
        await _db.SaveChangesAsync(ct);
        return unread.Count;
    }

    public Task<int> GetUnreadCountAsync(Guid currentUserId, CancellationToken ct = default)
        => _db.ChatMessages.AsNoTracking().CountAsync(m => m.RecipientId == currentUserId && !m.IsRead, ct);

    private static ChatMessageResponse Map(ChatMessage m, string senderName, bool senderHasImage) => new()
    {
        Id = m.Id,
        SenderId = m.SenderId,
        SenderName = senderName,
        SenderHasProfileImage = senderHasImage,
        RecipientId = m.RecipientId,
        Text = m.Text,
        SentAt = m.CreatedAt,
        IsRead = m.IsRead,
        HasAttachment = m.AttachmentData != null || !string.IsNullOrEmpty(m.AttachmentFileName),
        AttachmentFileName = m.AttachmentFileName,
        AttachmentContentType = m.AttachmentContentType
    };
}

