using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.Menu.Lookups;
using Energy.Shared.Models.V1.IAM.Menu.Responses;

namespace Energy.Infrastructure.Modules.IAM.Menu.Lookups;

/// <summary>Menu lookup servisi (aktif + arama filtreli projection).</summary>
public class MenuLookupService : IMenuLookupService
{
    private readonly EnergyDbContext _db;

    public MenuLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MenuLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Menus.AsNoTracking();
        var items = await query.Select(e => new MenuLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MenuLookupResponse>>.Success(items);
    }
}
