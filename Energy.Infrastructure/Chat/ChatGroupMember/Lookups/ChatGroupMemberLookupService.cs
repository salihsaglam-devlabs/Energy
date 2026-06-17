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
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ChatGroupMemberLookupResponse>)rows.Select(e => new ChatGroupMemberLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Status.ToString()) ? "Chat Group Member #" + e.Id.ToString().Substring(0, 8) : (e.Status.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ChatGroupMemberLookupResponse>>.Success(items);
    }
}
