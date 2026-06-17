using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Contracts.ContractAmendment.Lookups;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;

namespace Energy.Infrastructure.Contracts.ContractAmendment.Lookups;

/// <summary>ContractAmendment lookup servisi (aktif + arama filtreli projection).</summary>
public class ContractAmendmentLookupService : IContractAmendmentLookupService
{
    private readonly AppDbContext _db;

    public ContractAmendmentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ContractAmendments.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.AmendmentNo)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ContractAmendmentLookupResponse>)rows.Select(e => new ContractAmendmentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.AmendmentNo ?? "") + " - " + (e.Description ?? "")) ? "Contract Amendment #" + e.Id.ToString().Substring(0, 8) : ((e.AmendmentNo ?? "") + " - " + (e.Description ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>.Success(items);
    }
}
