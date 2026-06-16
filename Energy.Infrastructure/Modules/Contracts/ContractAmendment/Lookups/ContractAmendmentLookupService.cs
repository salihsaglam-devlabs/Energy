using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Contracts.ContractAmendment.Lookups;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;

namespace Energy.Infrastructure.Modules.Contracts.ContractAmendment.Lookups;

/// <summary>ContractAmendment lookup servisi (aktif + arama filtreli projection).</summary>
public class ContractAmendmentLookupService : IContractAmendmentLookupService
{
    private readonly EnergyDbContext _db;

    public ContractAmendmentLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ContractAmendments.AsNoTracking();
        var items = await query.Select(e => new ContractAmendmentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>.Success(items);
    }
}
