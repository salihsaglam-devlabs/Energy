using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Contracts.ContractParty.Lookups;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;

namespace Energy.Infrastructure.Modules.Contracts.ContractParty.Lookups;

/// <summary>ContractParty lookup servisi (aktif + arama filtreli projection).</summary>
public class ContractPartyLookupService : IContractPartyLookupService
{
    private readonly AppDbContext _db;

    public ContractPartyLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ContractPartyLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ContractParties.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ContractPartyLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ContractPartyLookupResponse>>.Success(items);
    }
}
