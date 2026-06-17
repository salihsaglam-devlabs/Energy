using ChatGroupEntity = Energy.Domain.Chat.ChatGroup;
using ChatGroupMemberEntity = Energy.Domain.Chat.ChatGroupMember;
using ChatGroupMemberStatusEntity = Energy.Domain.Chat.ChatGroupMemberStatus;
using ChatMessageEntity = Energy.Domain.Chat.ChatMessage;
using ChatMessageReactionEntity = Energy.Domain.Chat.ChatMessageReaction;
using Energy.Application.Chat.Services;
using Energy.Domain.Chat;
using Energy.Domain.IAM;
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
            .Where(m => m.GroupId == null && m.RecipientId == currentUserId && !m.IsRead)
            .GroupBy(m => m.SenderId)
            .Select(g => new { SenderId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SenderId, x => x.Count, ct);

        var lastByPeer = await _db.ChatMessages.AsNoTracking()
            .Where(m => m.GroupId == null && (m.SenderId == currentUserId || m.RecipientId == currentUserId))
            .GroupBy(m => m.SenderId == currentUserId ? m.RecipientId!.Value : m.SenderId)
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
        // Yalnızca üst veriyi (asla ekin baytları değil) yansıtırız; böylece uzun
        // bir konuşmayı listelemek ucuz kalır; baytlar GetAttachmentAsync ile
        // istendiğinde akıtılır.
        var rows = await _db.ChatMessages.AsNoTracking()
            .Where(m => m.GroupId == null
                     && ((m.SenderId == currentUserId && m.RecipientId == peerId)
                      || (m.SenderId == peerId && m.RecipientId == currentUserId)))
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
                    ReplyToId = m.ReplyToMessageId,
                    HasAttachment = m.AttachmentData != null,
                    AttachmentFileName = m.AttachmentFileName,
                    AttachmentContentType = m.AttachmentContentType
                })
            .ToListAsync(ct);

        await EnrichAsync(rows, currentUserId, ct);
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

        var message = new ChatMessageEntity
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            RecipientId = request.GroupId.HasValue ? null : request.RecipientId,
            GroupId = request.GroupId,
            Text = (request.Text ?? string.Empty).Trim(),
            IsRead = false,
            ReplyToMessageId = request.ReplyToMessageId,
            AttachmentFileName = attachmentName,
            AttachmentContentType = attachmentContentType,
            AttachmentData = attachmentData
        };

        // Grup mesajları kabul edilmiş bir üyelik; doğrudan mesajlar bir alıcı gerektirir.
        if (request.GroupId is { } groupId)
        {
            var isMember = await _db.ChatGroupMembers.AsNoTracking().AnyAsync(
                gm => gm.GroupId == groupId && gm.UserId == senderId && gm.Status == ChatGroupMemberStatusEntity.Accepted, ct);
            if (!isMember)
            {
                throw new InvalidOperationException("Not a member of the group.");
            }
        }

        _db.ChatMessages.Add(message);
        await _db.SaveChangesAsync(ct);

        var sender = await _db.Users.AsNoTracking()
            .Where(u => u.Id == senderId)
            .Select(u => new { Name = $"{u.FirstName} {u.LastName}".Trim(), HasImage = u.ProfileImage != null })
            .FirstOrDefaultAsync(ct);

        var response = Map(message, sender?.Name ?? string.Empty, sender?.HasImage ?? false);
        await EnrichAsync(new[] { response }, senderId, ct);
        return response;
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
            .Where(m => m.GroupId == null && m.RecipientId == currentUserId && m.SenderId == peerId && !m.IsRead)
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
        => _db.ChatMessages.AsNoTracking().CountAsync(m => m.GroupId == null && m.RecipientId == currentUserId && !m.IsRead, ct);

    // ----- Groups -----------------------------------------------------------

    public async Task<IReadOnlyList<ChatGroupResponse>> GetGroupsAsync(Guid currentUserId, CancellationToken ct = default)
    {
        var myMemberships = await _db.ChatGroupMembers.AsNoTracking()
            .Where(gm => gm.UserId == currentUserId && gm.Status == ChatGroupMemberStatusEntity.Accepted)
            .Select(gm => new { gm.GroupId, gm.IsOwner, gm.IsAdmin })
            .ToListAsync(ct);
        if (myMemberships.Count == 0) return [];

        var myGroupIds = myMemberships.Select(m => m.GroupId).ToList();
        var manageById = myMemberships.ToDictionary(m => m.GroupId, m => m.IsOwner || m.IsAdmin);

        var groups = await _db.ChatGroups.AsNoTracking()
            .Where(g => myGroupIds.Contains(g.Id))
            .ToListAsync(ct);

        var memberCounts = await _db.ChatGroupMembers.AsNoTracking()
            .Where(gm => myGroupIds.Contains(gm.GroupId) && gm.Status == ChatGroupMemberStatusEntity.Accepted)
            .GroupBy(gm => gm.GroupId)
            .Select(g => new { GroupId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.GroupId, x => x.Count, ct);

        var lastByGroup = await _db.ChatMessages.AsNoTracking()
            .Where(m => m.GroupId != null && myGroupIds.Contains(m.GroupId!.Value))
            .GroupBy(m => m.GroupId!.Value)
            .Select(g => new { GroupId = g.Key, LastAt = g.Max(m => m.CreatedAt) })
            .ToDictionaryAsync(x => x.GroupId, x => x.LastAt, ct);

        return groups
            .Select(g => new ChatGroupResponse
            {
                Id = g.Id,
                Name = g.Name,
                OwnerId = g.OwnerId,
                IsOwner = g.OwnerId == currentUserId,
                IsAdmin = manageById.GetValueOrDefault(g.Id),
                MemberCount = memberCounts.GetValueOrDefault(g.Id),
                UnreadCount = 0,
                LastMessageAt = lastByGroup.TryGetValue(g.Id, out var at) ? at : null
            })
            .OrderByDescending(g => g.LastMessageAt ?? DateTime.MinValue)
            .ThenBy(g => g.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task<IReadOnlyList<ChatGroupInviteResponse>> GetGroupInvitesAsync(Guid currentUserId, CancellationToken ct = default)
    {
        var invites = await _db.ChatGroupMembers.AsNoTracking()
            .Where(gm => gm.UserId == currentUserId && gm.Status == ChatGroupMemberStatusEntity.Pending)
            .Join(_db.ChatGroups.AsNoTracking(), gm => gm.GroupId, g => g.Id, (gm, g) => new { gm, g })
            .ToListAsync(ct);

        if (invites.Count == 0) return [];

        var inviterIds = invites.Where(x => x.gm.InvitedById.HasValue).Select(x => x.gm.InvitedById!.Value).Distinct().ToList();
        var inviterNames = await _db.Users.AsNoTracking()
            .Where(u => inviterIds.Contains(u.Id))
            .Select(u => new { u.Id, Name = ((u.FirstName ?? "") + " " + (u.LastName ?? "")).Trim() })
            .ToDictionaryAsync(x => x.Id, x => x.Name, ct);

        return invites
            .Select(x => new ChatGroupInviteResponse
            {
                GroupId = x.g.Id,
                GroupName = x.g.Name,
                OwnerId = x.g.OwnerId,
                InvitedByName = x.gm.InvitedById.HasValue ? inviterNames.GetValueOrDefault(x.gm.InvitedById.Value, "") : "",
                InvitedAt = x.gm.CreatedAt
            })
            .OrderByDescending(i => i.InvitedAt)
            .ToList();
    }

    public async Task<ChatGroupResponse> CreateGroupAsync(Guid ownerId, CreateChatGroupRequest request, CancellationToken ct = default)
    {
        var name = (request.Name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Group name is required.");
        }

        var group = new ChatGroupEntity { Id = Guid.NewGuid(), Name = name, OwnerId = ownerId };
        _db.ChatGroups.Add(group);

        // Owner is an immediate accepted member.
        _db.ChatGroupMembers.Add(new ChatGroupMemberEntity
        {
            Id = Guid.NewGuid(),
            GroupId = group.Id,
            UserId = ownerId,
            Status = ChatGroupMemberStatusEntity.Accepted,
            IsOwner = true
        });

        // Davet edilen kullanıcılar beklemede üyelik alır (yalnızca kabul ettikten sonra etkin olur).
        foreach (var userId in (request.MemberUserIds ?? []).Distinct().Where(id => id != ownerId && id != Guid.Empty))
        {
            _db.ChatGroupMembers.Add(new ChatGroupMemberEntity
            {
                Id = Guid.NewGuid(),
                GroupId = group.Id,
                UserId = userId,
                Status = ChatGroupMemberStatusEntity.Pending,
                InvitedById = ownerId
            });
        }

        await _db.SaveChangesAsync(ct);

        return new ChatGroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            OwnerId = ownerId,
            IsOwner = true,
            MemberCount = 1,
            UnreadCount = 0,
            LastMessageAt = null
        };
    }

    public async Task<IReadOnlyList<Guid>> InviteToGroupAsync(Guid currentUserId, Guid groupId, InviteToGroupRequest request, CancellationToken ct = default)
    {
        // Davet eden kişi, grubun kabul edilmiş bir üyesi olmalıdır.
        var isMember = await _db.ChatGroupMembers.AsNoTracking().AnyAsync(
            gm => gm.GroupId == groupId && gm.UserId == currentUserId && gm.Status == ChatGroupMemberStatusEntity.Accepted, ct);
        if (!isMember)
        {
            throw new InvalidOperationException("Not a member of the group.");
        }

        var existing = await _db.ChatGroupMembers
            .Where(gm => gm.GroupId == groupId)
            .ToListAsync(ct);
        var existingByUser = existing.ToDictionary(gm => gm.UserId, gm => gm);

        var invited = new List<Guid>();
        foreach (var userId in (request.UserIds ?? []).Distinct().Where(id => id != Guid.Empty))
        {
            if (existingByUser.TryGetValue(userId, out var row))
            {
                // Daha önce reddetmiş/çıkarılmış bir kullanıcıyı yeniden davet et.
                if (row.Status == ChatGroupMemberStatusEntity.Declined)
                {
                    row.Status = ChatGroupMemberStatusEntity.Pending;
                    row.InvitedById = currentUserId;
                    row.UpdatedAt = DateTime.UtcNow;
                    invited.Add(userId);
                }
                continue;
            }

            _db.ChatGroupMembers.Add(new ChatGroupMemberEntity
            {
                Id = Guid.NewGuid(),
                GroupId = groupId,
                UserId = userId,
                Status = ChatGroupMemberStatusEntity.Pending,
                InvitedById = currentUserId
            });
            invited.Add(userId);
        }

        await _db.SaveChangesAsync(ct);
        return invited;
    }

    public async Task<bool> RespondInviteAsync(Guid currentUserId, Guid groupId, bool accept, CancellationToken ct = default)
    {
        var row = await _db.ChatGroupMembers
            .FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == currentUserId && gm.Status == ChatGroupMemberStatusEntity.Pending, ct);
        if (row is null)
        {
            return false;
        }

        row.Status = accept ? ChatGroupMemberStatusEntity.Accepted : ChatGroupMemberStatusEntity.Declined;
        row.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<ChatGroupMemberResponse>> GetGroupMembersAsync(Guid currentUserId, Guid groupId, CancellationToken ct = default)
    {
        var isMember = await _db.ChatGroupMembers.AsNoTracking().AnyAsync(
            gm => gm.GroupId == groupId && gm.UserId == currentUserId && gm.Status == ChatGroupMemberStatusEntity.Accepted, ct);
        if (!isMember)
        {
            return [];
        }

        return await _db.ChatGroupMembers.AsNoTracking()
            .Where(gm => gm.GroupId == groupId && gm.Status != ChatGroupMemberStatusEntity.Declined)
            .Join(_db.Users.AsNoTracking(), gm => gm.UserId, u => u.Id, (gm, u) => new ChatGroupMemberResponse
            {
                UserId = u.Id,
                FullName = ((u.FirstName ?? "") + " " + (u.LastName ?? "")).Trim(),
                UserName = u.UserName,
                HasProfileImage = u.ProfileImage != null,
                IsOwner = gm.IsOwner,
                IsAdmin = gm.IsOwner || gm.IsAdmin,
                Status = (int)gm.Status
            })
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Guid>> GetGroupMemberIdsAsync(Guid groupId, CancellationToken ct = default)
        => await _db.ChatGroupMembers.AsNoTracking()
            .Where(gm => gm.GroupId == groupId && gm.Status == ChatGroupMemberStatusEntity.Accepted)
            .Select(gm => gm.UserId)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ChatMessageResponse>> GetGroupConversationAsync(Guid currentUserId, Guid groupId, CancellationToken ct = default)
    {
        var isMember = await _db.ChatGroupMembers.AsNoTracking().AnyAsync(
            gm => gm.GroupId == groupId && gm.UserId == currentUserId && gm.Status == ChatGroupMemberStatusEntity.Accepted, ct);
        if (!isMember)
        {
            return [];
        }

        var rows = await _db.ChatMessages.AsNoTracking()
            .Where(m => m.GroupId == groupId)
            .OrderBy(m => m.CreatedAt)
            .Join(_db.Users.AsNoTracking(), m => m.SenderId, u => u.Id, (m, u) => new ChatMessageResponse
            {
                Id = m.Id,
                SenderId = m.SenderId,
                SenderName = ((u.FirstName ?? "") + " " + (u.LastName ?? "")).Trim(),
                SenderHasProfileImage = u.ProfileImage != null,
                RecipientId = null,
                GroupId = m.GroupId,
                Text = m.Text,
                SentAt = m.CreatedAt,
                IsRead = m.IsRead,
                ReplyToId = m.ReplyToMessageId,
                HasAttachment = m.AttachmentData != null,
                AttachmentFileName = m.AttachmentFileName,
                AttachmentContentType = m.AttachmentContentType
            })
            .ToListAsync(ct);

        await EnrichAsync(rows, currentUserId, ct);
        return rows;
    }

    // ----- Grup yönetimi (sahip/yönetici) ----------------------------------

    // Kullanıcı, grubun kabul edilmiş sahibi veya yöneticisiyse true döner.
    private Task<bool> IsManagerAsync(Guid userId, Guid groupId, CancellationToken ct)
        => _db.ChatGroupMembers.AsNoTracking().AnyAsync(
            gm => gm.GroupId == groupId
               && gm.UserId == userId
               && gm.Status == ChatGroupMemberStatusEntity.Accepted
               && (gm.IsOwner || gm.IsAdmin), ct);

    public async Task<bool> DeleteGroupAsync(Guid currentUserId, Guid groupId, CancellationToken ct = default)
    {
        if (!await IsManagerAsync(currentUserId, groupId, ct))
        {
            return false;
        }

        var group = await _db.ChatGroups.FirstOrDefaultAsync(g => g.Id == groupId, ct);
        if (group is null)
        {
            return false;
        }

        // Grubu, tüm üyeliklerini ve tüm mesajlarını yumuşak sil (interceptor,
        // Remove sırasında IsDeleted'i true yapar; sorgu filtreleri de bunları gizler).
        var members = await _db.ChatGroupMembers.Where(gm => gm.GroupId == groupId).ToListAsync(ct);
        _db.ChatGroupMembers.RemoveRange(members);

        var messages = await _db.ChatMessages.Where(m => m.GroupId == groupId).ToListAsync(ct);
        _db.ChatMessages.RemoveRange(messages);

        _db.ChatGroups.Remove(group);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> RemoveMemberAsync(Guid currentUserId, Guid groupId, Guid memberUserId, CancellationToken ct = default)
    {
        if (!await IsManagerAsync(currentUserId, groupId, ct))
        {
            return false;
        }

        var row = await _db.ChatGroupMembers
            .FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == memberUserId, ct);
        if (row is null || row.IsOwner)
        {
            // Grup sahibi asla çıkarılamaz.
            return false;
        }

        _db.ChatGroupMembers.Remove(row);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> SetMemberAdminAsync(Guid currentUserId, Guid groupId, Guid memberUserId, bool isAdmin, CancellationToken ct = default)
    {
        if (!await IsManagerAsync(currentUserId, groupId, ct))
        {
            return false;
        }

        var row = await _db.ChatGroupMembers
            .FirstOrDefaultAsync(gm => gm.GroupId == groupId
                                    && gm.UserId == memberUserId
                                    && gm.Status == ChatGroupMemberStatusEntity.Accepted, ct);
        if (row is null || row.IsOwner)
        {
            // Grup sahibi her zaman yöneticidir; durumu değiştirilemez.
            return false;
        }

        if (row.IsAdmin == isAdmin)
        {
            return true;
        }

        row.IsAdmin = isAdmin;
        row.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    // ----- Delete / Forward / Reactions ------------------------------------

    public async Task<ChatMessageResponse?> DeleteMessageAsync(Guid currentUserId, Guid messageId, CancellationToken ct = default)
    {
        var m = await _db.ChatMessages.FirstOrDefaultAsync(x => x.Id == messageId, ct);
        if (m is null || m.SenderId != currentUserId)
        {
            return null;
        }

        m.IsDeleted = true;
        m.DeletedAt = DateTime.UtcNow;
        m.DeletedBy = currentUserId;
        await _db.SaveChangesAsync(ct);

        return new ChatMessageResponse
        {
            Id = m.Id,
            SenderId = m.SenderId,
            RecipientId = m.RecipientId,
            GroupId = m.GroupId,
            IsDeleted = true
        };
    }

    public async Task<ChatMessageResponse?> ForwardAsync(Guid currentUserId, ForwardChatMessageRequest request, CancellationToken ct = default)
    {
        var src = await _db.ChatMessages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.MessageId, ct);
        if (src is null || !await CanAccessMessageAsync(currentUserId, src, ct))
        {
            return null;
        }

        if (request.GroupId is { } targetGroup)
        {
            var isMember = await _db.ChatGroupMembers.AsNoTracking().AnyAsync(
                gm => gm.GroupId == targetGroup && gm.UserId == currentUserId && gm.Status == ChatGroupMemberStatusEntity.Accepted, ct);
            if (!isMember)
            {
                return null;
            }
        }
        else if (request.RecipientId is null)
        {
            return null;
        }

        var fwd = new ChatMessageEntity
        {
            Id = Guid.NewGuid(),
            SenderId = currentUserId,
            RecipientId = request.GroupId.HasValue ? null : request.RecipientId,
            GroupId = request.GroupId,
            Text = src.Text,
            IsRead = false,
            AttachmentFileName = src.AttachmentFileName,
            AttachmentContentType = src.AttachmentContentType,
            AttachmentData = src.AttachmentData
        };
        _db.ChatMessages.Add(fwd);
        await _db.SaveChangesAsync(ct);

        var sender = await _db.Users.AsNoTracking()
            .Where(u => u.Id == currentUserId)
            .Select(u => new { Name = $"{u.FirstName} {u.LastName}".Trim(), HasImage = u.ProfileImage != null })
            .FirstOrDefaultAsync(ct);
        return Map(fwd, sender?.Name ?? string.Empty, sender?.HasImage ?? false);
    }

    public async Task<ChatMessageResponse?> ToggleReactionAsync(Guid currentUserId, Guid messageId, string emoji, CancellationToken ct = default)
    {
        emoji = (emoji ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(emoji))
        {
            return null;
        }

        var msg = await _db.ChatMessages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == messageId, ct);
        if (msg is null || !await CanAccessMessageAsync(currentUserId, msg, ct))
        {
            return null;
        }

        var existing = await _db.ChatMessageReactions
            .FirstOrDefaultAsync(r => r.MessageId == messageId && r.UserId == currentUserId, ct);
        if (existing is null)
        {
            _db.ChatMessageReactions.Add(new ChatMessageReactionEntity
            {
                Id = Guid.NewGuid(),
                MessageId = messageId,
                UserId = currentUserId,
                Emoji = emoji
            });
        }
        else if (string.Equals(existing.Emoji, emoji, StringComparison.Ordinal))
        {
            _db.ChatMessageReactions.Remove(existing); // aynı emojiyi geri kapat (toggle)
        }
        else
        {
            existing.Emoji = emoji;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync(ct);

        var sender = await _db.Users.AsNoTracking()
            .Where(u => u.Id == msg.SenderId)
            .Select(u => new { Name = $"{u.FirstName} {u.LastName}".Trim(), HasImage = u.ProfileImage != null })
            .FirstOrDefaultAsync(ct);
        var response = Map(msg, sender?.Name ?? string.Empty, sender?.HasImage ?? false);
        await EnrichAsync(new[] { response }, currentUserId, ct);
        return response;
    }

    // Kullanıcı, mesajın bir katılımcısıysa (doğrudan muhatap ya da kabul edilmiş grup üyesi) true döner.
    private async Task<bool> CanAccessMessageAsync(Guid userId, ChatMessageEntity m, CancellationToken ct)
    {
        if (m.SenderId == userId || m.RecipientId == userId)
        {
            return true;
        }
        return m.GroupId.HasValue && await _db.ChatGroupMembers.AsNoTracking().AnyAsync(
            gm => gm.GroupId == m.GroupId && gm.UserId == userId && gm.Status == ChatGroupMemberStatusEntity.Accepted, ct);
    }

    // Verilen projeksiyonlara yanıt parçacıklarını + tepki özetlerini doldurur.
    private async Task EnrichAsync(IReadOnlyList<ChatMessageResponse> msgs, Guid currentUserId, CancellationToken ct)
    {
        if (msgs.Count == 0)
        {
            return;
        }

        var ids = msgs.Select(m => m.Id).ToList();

        var reactions = await _db.ChatMessageReactions.AsNoTracking()
            .Where(r => ids.Contains(r.MessageId))
            .Select(r => new { r.MessageId, r.UserId, r.Emoji })
            .ToListAsync(ct);
        var reactionsByMsg = reactions.GroupBy(r => r.MessageId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var replyIds = msgs.Where(m => m.ReplyToId.HasValue).Select(m => m.ReplyToId!.Value).Distinct().ToList();
        var replyMap = replyIds.Count == 0
            ? new Dictionary<Guid, (string Text, string Sender, string? File)>()
            : (await _db.ChatMessages.AsNoTracking()
                .Where(m => replyIds.Contains(m.Id))
                .Join(_db.Users.AsNoTracking(), m => m.SenderId, u => u.Id, (m, u) => new
                {
                    m.Id,
                    m.Text,
                    m.AttachmentFileName,
                    Sender = ((u.FirstName ?? "") + " " + (u.LastName ?? "")).Trim()
                })
                .ToListAsync(ct))
              .ToDictionary(x => x.Id, x => (Text: x.Text, Sender: x.Sender, File: (string?)x.AttachmentFileName));

        foreach (var m in msgs)
        {
            if (reactionsByMsg.TryGetValue(m.Id, out var rs))
            {
                m.Reactions = rs.GroupBy(r => r.Emoji)
                    .Select(g => new ChatReactionSummary
                    {
                        Emoji = g.Key,
                        Count = g.Count(),
                        Reacted = g.Any(x => x.UserId == currentUserId)
                    })
                    .ToList();
            }

            if (m.ReplyToId.HasValue && replyMap.TryGetValue(m.ReplyToId.Value, out var r))
            {
                m.ReplyToSenderName = r.Sender;
                m.ReplyToText = !string.IsNullOrEmpty(r.Text) ? r.Text : (r.File ?? string.Empty);
            }
        }
    }

    private static ChatMessageResponse Map(ChatMessageEntity m, string senderName, bool senderHasImage) => new()
    {
        Id = m.Id,
        SenderId = m.SenderId,
        SenderName = senderName,
        SenderHasProfileImage = senderHasImage,
        RecipientId = m.RecipientId,
        GroupId = m.GroupId,
        Text = m.Text,
        SentAt = m.CreatedAt,
        IsRead = m.IsRead,
        ReplyToId = m.ReplyToMessageId,
        HasAttachment = m.AttachmentData != null || !string.IsNullOrEmpty(m.AttachmentFileName),
        AttachmentFileName = m.AttachmentFileName,
        AttachmentContentType = m.AttachmentContentType
    };
}

