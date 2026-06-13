using Energy.Application.Logger.Services;
using Energy.Domain.Logger;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Logging;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Logger.Services;

public sealed class AuditLogService : IAuditLogService
{
    private const string WebSource = "Web";

    private readonly AppDbContext _db;

    public AuditLogService(AppDbContext db) { _db = db; }

    public async Task WriteAsync(AuditLog log, CancellationToken ct = default)
    {
        _db.AuditLogs.Add(log);
        await _db.SaveChangesAsync(ct);
    }

    public async Task IngestAsync(
        CreateAuditLogRequest request,
        Guid? userId,
        string? userName,
        string? ipAddress,
        CancellationToken ct = default)
    {
        // PostgreSQL "timestamp with time zone" DateTimeKind.Utc gerektirir; JSON'dan
        // ayrıştırılan bir değer Unspecified/Local olarak gelebilir ve kaydetme
        // sırasında hata fırlatır. Savunmacı şekilde normalize et.
        var occurredAt = request.OccurredAt == default ? DateTime.UtcNow : request.OccurredAt;
        occurredAt = occurredAt.Kind switch
        {
            DateTimeKind.Utc => occurredAt,
            DateTimeKind.Local => occurredAt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(occurredAt, DateTimeKind.Utc)
        };

        var log = new AuditLog
        {
            OccurredAt = occurredAt,
            UserId = userId,
            UserName = userName,
            IpAddress = ipAddress,
            HttpMethod = request.HttpMethod,
            Path = request.Path,
            QueryString = SensitiveDataMasker.MaskQueryString(request.QueryString),
            StatusCode = request.StatusCode,
            IsSuccess = request.IsSuccess,
            Source = WebSource,
            RequestBody = SensitiveDataMasker.MaskBody(request.RequestBody),
            ResponseBody = SensitiveDataMasker.MaskBody(request.ResponseBody),
            HasException = request.HasException,
            ExceptionType = request.ExceptionType,
            ExceptionMessage = request.ExceptionMessage,
            CorrelationId = request.CorrelationId,
            DurationMs = request.DurationMs
        };
        _db.AuditLogs.Add(log);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<PaginatedResponse<AuditLogResponse>> QueryAsync(
        AuditLogQueryRequest query, PaginatedRequest paging, CancellationToken ct = default)
    {
        var q = _db.AuditLogs.AsNoTracking().AsQueryable();
        if (query.FromUtc.HasValue) q = q.Where(l => l.OccurredAt >= query.FromUtc);
        if (query.ToUtc.HasValue) q = q.Where(l => l.OccurredAt <= query.ToUtc);
        if (query.UserId.HasValue) q = q.Where(l => l.UserId == query.UserId);
        if (!string.IsNullOrWhiteSpace(query.IpAddress)) q = q.Where(l => l.IpAddress == query.IpAddress);
        if (!string.IsNullOrWhiteSpace(query.HttpMethod)) q = q.Where(l => l.HttpMethod == query.HttpMethod.ToUpper());
        if (!string.IsNullOrWhiteSpace(query.PathContains)) q = q.Where(l => l.Path!.Contains(query.PathContains));
        if (query.StatusCode.HasValue) q = q.Where(l => l.StatusCode == query.StatusCode);
        if (query.IsSuccess.HasValue) q = q.Where(l => l.IsSuccess == query.IsSuccess);
        if (query.HasException.HasValue) q = q.Where(l => l.HasException == query.HasException);
        if (query.CorrelationId.HasValue) q = q.Where(l => l.CorrelationId == query.CorrelationId);
        if (!string.IsNullOrWhiteSpace(query.Source)) q = q.Where(l => l.Source == query.Source);

        var total = await q.CountAsync(ct);
        var page = await q
            .OrderByDescending(l => l.OccurredAt)
            .Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize)
            .Select(l => Project(l))
            .ToListAsync(ct);
        return PaginatedResponse<AuditLogResponse>.Create(page, paging.PageNumber, paging.PageSize, total);
    }

    public async Task<AuditLogResponse?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _db.AuditLogs.AsNoTracking().Where(l => l.Id == id).Select(l => Project(l)).FirstOrDefaultAsync(ct);
    }

    private static AuditLogResponse Project(AuditLog l) => new()
    {
        Id = l.Id,
        OccurredAt = l.OccurredAt,
        UserId = l.UserId,
        UserName = l.UserName,
        IpAddress = l.IpAddress,
        HttpMethod = l.HttpMethod,
        Path = l.Path,
        QueryString = l.QueryString,
        StatusCode = l.StatusCode,
        IsSuccess = l.IsSuccess,
        Source = l.Source,
        RequestBody = l.RequestBody,
        ResponseBody = l.ResponseBody,
        HasException = l.HasException,
        ExceptionType = l.ExceptionType,
        ExceptionMessage = l.ExceptionMessage,
        CorrelationId = l.CorrelationId,
        DurationMs = l.DurationMs
    };
}
