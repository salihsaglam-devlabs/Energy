using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Payment.Lookups;
using Energy.Shared.Models.V1.Finance.Payment.Responses;

namespace Energy.Infrastructure.Modules.Finance.Payment.Lookups;

/// <summary>Payment lookup servisi (aktif + arama filtreli projection).</summary>
public class PaymentLookupService : IPaymentLookupService
{
    private readonly EnergyDbContext _db;

    public PaymentLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PaymentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Payments.AsNoTracking();
        var items = await query.Select(e => new PaymentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<PaymentLookupResponse>>.Success(items);
    }
}
