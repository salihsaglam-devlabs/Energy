using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.AuditLog.Services;
using Energy.Shared.Models.V1.Core.AuditLog.Requests;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;

namespace Energy.Infrastructure.Modules.Core.AuditLog.Services;

/// <summary>
/// AuditLog yalnızca-ekleme (append-only) denetim kaydıdır. Kayıtlar logging
/// altyapısı tarafından üretilir; arayüzden oluşturulmaz/güncellenmez/silinmez.
/// Bu servis salt-okunur liste sağlar (long anahtar surrogate Guid'e eşlenmez).
/// </summary>
public class AuditLogService : IAuditLogService
{
    private readonly AppDbContext _db;

    public AuditLogService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<AuditLogListResponse>>> GetListAsync(GetAuditLogListRequest request, CancellationToken ct = default)
    {
        var query = _db.AuditLogs.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.OccurredAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new AuditLogListResponse
            {
                Id = Guid.Empty,
                OccurredAt = e.OccurredAt,
                UserId = e.UserId,
                UserName = e.UserName,
                IpAddress = e.IpAddress,
                HttpMethod = e.HttpMethod,
                Path = e.Path,
                QueryString = e.QueryString,
                StatusCode = e.StatusCode,
                IsSuccess = e.IsSuccess,
                Source = e.Source,
                RequestBody = e.RequestBody,
                ResponseBody = e.ResponseBody,
                HasException = e.HasException,
                ExceptionType = e.ExceptionType,
                ExceptionMessage = e.ExceptionMessage,
                CorrelationId = e.CorrelationId,
                DurationMs = e.DurationMs
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<AuditLogListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<AuditLogListResponse>>.Success(page);
    }

    public Task<BaseResponse<AuditLogDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<AuditLogDetailResponse>.Failure("NotSupported"));

    public Task<BaseResponse<Guid>> CreateAsync(CreateAuditLogRequest request, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<Guid>.Failure("NotSupported"));

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateAuditLogRequest request, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));
}
