using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.IAM.Menu.Lookups;
using Energy.Shared.Models.V1.IAM.Menu.Responses;

namespace Energy.Infrastructure.IAM.Menu.Lookups;

/// <summary>Menu lookup servisi (aktif + arama filtreli projection).</summary>
public class MenuLookupService : IMenuLookupService
{
    private readonly AppDbContext _db;

    public MenuLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MenuLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Menus.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new MenuLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MenuLookupResponse>>.Success(items);
    }
}
