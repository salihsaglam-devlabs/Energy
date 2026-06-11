using Energy.Domain.Logger;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;

namespace Energy.Application.Logger.Services;

public interface IAuditLogService
{
    Task WriteAsync(AuditLog log, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists an audit entry submitted by an upper layer (Web). Identity and
    /// source are stamped server-side; bodies are re-masked before storage.
    /// </summary>
    Task IngestAsync(
        CreateAuditLogRequest request,
        Guid? userId,
        string? userName,
        string? ipAddress,
        CancellationToken cancellationToken = default);

    Task<PaginatedResponse<AuditLogResponse>> QueryAsync(AuditLogQueryRequest query, PaginatedRequest paging, CancellationToken cancellationToken = default);

    Task<AuditLogResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
