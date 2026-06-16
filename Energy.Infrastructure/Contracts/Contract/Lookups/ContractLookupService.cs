using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Contracts.Contract.Lookups;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;

namespace Energy.Infrastructure.Contracts.Contract.Lookups;

/// <summary>Contract lookup servisi (aktif + arama filtreli projection).</summary>
public class ContractLookupService : IContractLookupService
{
    private readonly AppDbContext _db;

    public ContractLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ContractLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Contracts.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Title.Contains(search));
        var items = await query
            .OrderBy(e => e.Title)
            .Select(e => new ContractLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Title,
                DisplayName = e.Title,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ContractLookupResponse>>.Success(items);
    }
}
