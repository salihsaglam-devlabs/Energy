using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Chat.ChatMessage.Lookups;
using Energy.Shared.Models.V1.Chat.ChatMessage.Responses;

namespace Energy.Infrastructure.Modules.Chat.ChatMessage.Lookups;

/// <summary>ChatMessage lookup servisi (aktif + arama filtreli projection).</summary>
public class ChatMessageLookupService : IChatMessageLookupService
{
    private readonly EnergyDbContext _db;

    public ChatMessageLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ChatMessageLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ChatMessages.AsNoTracking();
        var items = await query.Select(e => new ChatMessageLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ChatMessageLookupResponse>>.Success(items);
    }
}
