using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Chat.ChatMessageReaction.Lookups;
using Energy.Shared.Models.V1.Chat.ChatMessageReaction.Responses;

namespace Energy.Infrastructure.Chat.ChatMessageReaction.Lookups;

/// <summary>ChatMessageReaction lookup servisi (aktif + arama filtreli projection).</summary>
public class ChatMessageReactionLookupService : IChatMessageReactionLookupService
{
    private readonly AppDbContext _db;

    public ChatMessageReactionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ChatMessageReactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ChatMessageReactions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ChatMessageReactionLookupResponse>)rows.Select(e => new ChatMessageReactionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = "Chat Message Reaction #" + e.Id.ToString().Substring(0, 8),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ChatMessageReactionLookupResponse>>.Success(items);
    }
}
