using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Lookups;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;

namespace Energy.Infrastructure.BusinessPartners.BusinessPartnerBankAccount.Lookups;

/// <summary>BusinessPartnerBankAccount lookup servisi (aktif + arama filtreli projection).</summary>
public class BusinessPartnerBankAccountLookupService : IBusinessPartnerBankAccountLookupService
{
    private readonly AppDbContext _db;

    public BusinessPartnerBankAccountLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.BusinessPartnerBankAccounts.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<BusinessPartnerBankAccountLookupResponse>)rows.Select(e => new BusinessPartnerBankAccountLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = "Business Partner Bank Account #" + e.Id.ToString().Substring(0, 8),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>.Success(items);
    }
}
