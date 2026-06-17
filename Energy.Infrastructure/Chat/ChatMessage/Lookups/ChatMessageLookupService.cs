using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Chat.ChatMessage.Lookups;
using Energy.Shared.Models.V1.Chat.ChatMessage.Responses;

namespace Energy.Infrastructure.Chat.ChatMessage.Lookups;

/// <summary>ChatMessage lookup servisi (aktif + arama filtreli projection).</summary>
public class ChatMessageLookupService : IChatMessageLookupService
{
    private readonly AppDbContext _db;

    public ChatMessageLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ChatMessageLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ChatMessages.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ChatMessageLookupResponse>)rows.Select(e => new ChatMessageLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.AttachmentContentType ?? "") + " - " + (e.ReadAt.HasValue ? e.ReadAt.Value.ToString("yyyy-MM-dd") : "")) ? "Chat Message #" + e.Id.ToString().Substring(0, 8) : ((e.AttachmentContentType ?? "") + " - " + (e.ReadAt.HasValue ? e.ReadAt.Value.ToString("yyyy-MM-dd") : "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ChatMessageLookupResponse>>.Success(items);
    }
}
