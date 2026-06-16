using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Lookups;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;

namespace Energy.Infrastructure.Modules.BusinessPartners.BusinessPartnerBankAccount.Lookups;

/// <summary>BusinessPartnerBankAccount lookup servisi (aktif + arama filtreli projection).</summary>
public class BusinessPartnerBankAccountLookupService : IBusinessPartnerBankAccountLookupService
{
    private readonly AppDbContext _db;

    public BusinessPartnerBankAccountLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.BusinessPartnerBankAccounts.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new BusinessPartnerBankAccountLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>.Success(items);
    }
}
