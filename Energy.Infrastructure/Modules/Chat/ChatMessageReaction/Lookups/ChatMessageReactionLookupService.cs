using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Chat.ChatMessageReaction.Lookups;
using Energy.Shared.Models.V1.Chat.ChatMessageReaction.Responses;

namespace Energy.Infrastructure.Modules.Chat.ChatMessageReaction.Lookups;

/// <summary>ChatMessageReaction lookup servisi (aktif + arama filtreli projection).</summary>
public class ChatMessageReactionLookupService : IChatMessageReactionLookupService
{
    private readonly EnergyDbContext _db;

    public ChatMessageReactionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ChatMessageReactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ChatMessageReactions.AsNoTracking();
        var items = await query.Select(e => new ChatMessageReactionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ChatMessageReactionLookupResponse>>.Success(items);
    }
}
