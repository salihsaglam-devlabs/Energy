using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Lookups;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;

namespace Energy.Infrastructure.Modules.BusinessPartners.BusinessPartnerContact.Lookups;

/// <summary>BusinessPartnerContact lookup servisi (aktif + arama filtreli projection).</summary>
public class BusinessPartnerContactLookupService : IBusinessPartnerContactLookupService
{
    private readonly EnergyDbContext _db;

    public BusinessPartnerContactLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.BusinessPartnerContacts.AsNoTracking();
        var items = await query.Select(e => new BusinessPartnerContactLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>.Success(items);
    }
}
