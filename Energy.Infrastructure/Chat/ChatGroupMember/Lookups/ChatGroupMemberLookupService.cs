using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Chat.ChatGroupMember.Lookups;
using Energy.Shared.Models.V1.Chat.ChatGroupMember.Responses;

namespace Energy.Infrastructure.Chat.ChatGroupMember.Lookups;

/// <summary>ChatGroupMember lookup servisi (aktif + arama filtreli projection).</summary>
public class ChatGroupMemberLookupService : IChatGroupMemberLookupService
{
    private readonly AppDbContext _db;

    public ChatGroupMemberLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ChatGroupMemberLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ChatGroupMembers.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ChatGroupMemberLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ChatGroupMemberLookupResponse>>.Success(items);
    }
}
