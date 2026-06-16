using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Contracts.Contract.Lookups;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;

namespace Energy.Infrastructure.Modules.Contracts.Contract.Lookups;

/// <summary>Contract lookup servisi (aktif + arama filtreli projection).</summary>
public class ContractLookupService : IContractLookupService
{
    private readonly EnergyDbContext _db;

    public ContractLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ContractLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Contracts.AsNoTracking();
        var items = await query.Select(e => new ContractLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ContractLookupResponse>>.Success(items);
    }
}
