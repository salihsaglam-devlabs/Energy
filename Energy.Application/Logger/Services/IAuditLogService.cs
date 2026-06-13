using Energy.Domain.Logger;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;

namespace Energy.Application.Logger.Services;

/// <summary>Denetim (audit) günlüğü yazma ve sorgulama servisi.</summary>
public interface IAuditLogService
{
    /// <summary>Hazır bir denetim kaydını veritabanına yazar.</summary>
    Task WriteAsync(AuditLog log, CancellationToken cancellationToken = default);

    /// <summary>
    /// Üst katman (Web) tarafından gönderilen bir denetim kaydını saklar. Kimlik
    /// ve kaynak bilgisi sunucu tarafında damgalanır; gövdeler saklanmadan önce
    /// yeniden maskelenir.
    /// </summary>
    Task IngestAsync(
        CreateAuditLogRequest request,
        Guid? userId,
        string? userName,
        string? ipAddress,
        CancellationToken cancellationToken = default);

    /// <summary>Denetim kayıtlarını verilen filtre ve sayfalamaya göre sorgular.</summary>
    Task<PaginatedResponse<AuditLogResponse>> QueryAsync(AuditLogQueryRequest query, PaginatedRequest paging, CancellationToken cancellationToken = default);

    /// <summary>Belirtilen kimliğe sahip denetim kaydını döndürür; yoksa null.</summary>
    Task<AuditLogResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
