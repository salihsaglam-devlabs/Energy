using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.BusinessPartners.BusinessPartner.Lookups;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;

namespace Energy.Infrastructure.Modules.BusinessPartners.BusinessPartner.Lookups;

/// <summary>BusinessPartner lookup servisi (aktif + arama filtreli projection).</summary>
public class BusinessPartnerLookupService : IBusinessPartnerLookupService
{
    private readonly EnergyDbContext _db;

    public BusinessPartnerLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.BusinessPartners.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new BusinessPartnerLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>.Success(items);
    }
}
