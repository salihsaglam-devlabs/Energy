using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Organization.ExpenseClaim.Lookups;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;

namespace Energy.Infrastructure.Organization.ExpenseClaim.Lookups;

/// <summary>ExpenseClaim lookup servisi (aktif + arama filtreli projection).</summary>
public class ExpenseClaimLookupService : IExpenseClaimLookupService
{
    private readonly AppDbContext _db;

    public ExpenseClaimLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ExpenseClaims.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.ClaimNo.Contains(search));
        var items = await query
            .OrderBy(e => e.ClaimNo)
            .Select(e => new ExpenseClaimLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.ClaimNo,
                DisplayName = e.ClaimNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>.Success(items);
    }
}
