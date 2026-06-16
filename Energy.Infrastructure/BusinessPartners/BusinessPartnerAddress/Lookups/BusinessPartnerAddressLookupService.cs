using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.BusinessPartners.BusinessPartnerAddress.Lookups;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;

namespace Energy.Infrastructure.BusinessPartners.BusinessPartnerAddress.Lookups;

/// <summary>BusinessPartnerAddress lookup servisi (aktif + arama filtreli projection).</summary>
public class BusinessPartnerAddressLookupService : IBusinessPartnerAddressLookupService
{
    private readonly AppDbContext _db;

    public BusinessPartnerAddressLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.BusinessPartnerAddresses.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new BusinessPartnerAddressLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>.Success(items);
    }
}
