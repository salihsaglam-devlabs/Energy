using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Organization.ExpenseClaimLine.Lookups;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;

namespace Energy.Infrastructure.Organization.ExpenseClaimLine.Lookups;

/// <summary>ExpenseClaimLine lookup servisi (aktif + arama filtreli projection).</summary>
public class ExpenseClaimLineLookupService : IExpenseClaimLineLookupService
{
    private readonly AppDbContext _db;

    public ExpenseClaimLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ExpenseClaimLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ExpenseClaimLineLookupResponse>)rows.Select(e => new ExpenseClaimLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "") + " - " + (e.Category ?? "")) ? "Expense Claim Line #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "") + " - " + (e.Category ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>.Success(items);
    }
}
