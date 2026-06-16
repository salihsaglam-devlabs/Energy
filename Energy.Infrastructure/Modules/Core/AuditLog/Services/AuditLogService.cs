using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.AuditLog.Services;
using Energy.Shared.Models.V1.Core.AuditLog.Requests;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;

namespace Energy.Infrastructure.Modules.Core.AuditLog.Services;

/// <summary>AuditLog CRUD servisi (projection, pagination, soft-delete).</summary>
public class AuditLogService : IAuditLogService
{
    private readonly EnergyDbContext _db;

    public AuditLogService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<AuditLogListResponse>>> GetListAsync(GetAuditLogListRequest request, CancellationToken ct = default)
    {
        var query = _db.AuditLogs.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new AuditLogListResponse
            {
                Id = e.Id,
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
                DurationMs = e.DurationMs,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<AuditLogListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<AuditLogListResponse>>.Success(page);
    }

    public async Task<BaseResponse<AuditLogDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.AuditLogs.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new AuditLogDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
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
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<AuditLogDetailResponse>.Failure("NotFound")
            : BaseResponse<AuditLogDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateAuditLogRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Core.AuditLog
        {
            Id = Guid.NewGuid(),
            OccurredAt = request.OccurredAt,
            UserId = request.UserId,
            UserName = request.UserName,
            IpAddress = request.IpAddress,
            HttpMethod = request.HttpMethod,
            Path = request.Path,
            QueryString = request.QueryString,
            StatusCode = request.StatusCode,
            IsSuccess = request.IsSuccess,
            Source = request.Source,
            RequestBody = request.RequestBody,
            ResponseBody = request.ResponseBody,
            HasException = request.HasException,
            ExceptionType = request.ExceptionType,
            ExceptionMessage = request.ExceptionMessage,
            CorrelationId = request.CorrelationId,
            DurationMs = request.DurationMs,
            CreatedAt = DateTime.UtcNow,
        };
        _db.AuditLogs.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateAuditLogRequest request, CancellationToken ct = default)
    {
        var entity = await _db.AuditLogs.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.OccurredAt = request.OccurredAt;
            entity.UserId = request.UserId;
            entity.UserName = request.UserName;
            entity.IpAddress = request.IpAddress;
            entity.HttpMethod = request.HttpMethod;
            entity.Path = request.Path;
            entity.QueryString = request.QueryString;
            entity.StatusCode = request.StatusCode;
            entity.IsSuccess = request.IsSuccess;
            entity.Source = request.Source;
            entity.RequestBody = request.RequestBody;
            entity.ResponseBody = request.ResponseBody;
            entity.HasException = request.HasException;
            entity.ExceptionType = request.ExceptionType;
            entity.ExceptionMessage = request.ExceptionMessage;
            entity.CorrelationId = request.CorrelationId;
            entity.DurationMs = request.DurationMs;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.AuditLogs.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
