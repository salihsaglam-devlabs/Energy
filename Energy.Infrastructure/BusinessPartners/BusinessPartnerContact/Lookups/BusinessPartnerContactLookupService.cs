using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.BusinessPartners.BusinessPartnerContact.Lookups;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;

namespace Energy.Infrastructure.BusinessPartners.BusinessPartnerContact.Lookups;

/// <summary>BusinessPartnerContact lookup servisi (aktif + arama filtreli projection).</summary>
public class BusinessPartnerContactLookupService : IBusinessPartnerContactLookupService
{
    private readonly AppDbContext _db;

    public BusinessPartnerContactLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.BusinessPartnerContacts.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Title != null && e.Title.Contains(search));
        var items = await query
            .OrderBy(e => e.Title)
            .Select(e => new BusinessPartnerContactLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Title,
                DisplayName = e.Title ?? string.Empty,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>.Success(items);
    }
}
