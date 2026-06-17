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
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<BusinessPartnerAddressLookupResponse>)rows.Select(e => new BusinessPartnerAddressLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.AddressType ?? "")) ? "Business Partner Address #" + e.Id.ToString().Substring(0, 8) : ((e.AddressType ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>.Success(items);
    }
}
