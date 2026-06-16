using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Chat.ChatGroup.Lookups;
using Energy.Shared.Models.V1.Chat.ChatGroup.Responses;

namespace Energy.Infrastructure.Modules.Chat.ChatGroup.Lookups;

/// <summary>ChatGroup lookup servisi (aktif + arama filtreli projection).</summary>
public class ChatGroupLookupService : IChatGroupLookupService
{
    private readonly EnergyDbContext _db;

    public ChatGroupLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ChatGroupLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ChatGroups.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new ChatGroupLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ChatGroupLookupResponse>>.Success(items);
    }
}
