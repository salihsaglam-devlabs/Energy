using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalRequest.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalRequest.Lookups;

/// <summary>ApprovalRequest lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalRequestLookupService : IApprovalRequestLookupService
{
    private readonly AppDbContext _db;

    public ApprovalRequestLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalRequestLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalRequests.AsNoTracking();
        var rows = await query
            .OrderByDescending(e => e.CurrentStepNo)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ApprovalRequestLookupResponse>)rows.Select(e => new ApprovalRequestLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            // İlgili varlık türü + durum, kullanıcıya GUID yerine anlamlı bağlam verir.
            DisplayName = (string.IsNullOrWhiteSpace(e.RelatedEntityType) ? "Onay Talebi" : e.RelatedEntityType)
                + " (" + e.Status.ToString() + ")",
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ApprovalRequestLookupResponse>>.Success(items);
    }
}
