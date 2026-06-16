using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Requests;
using MediatR;

namespace Energy.Application.Modules.Core.AuditLog.Commands.CreateAuditLog;

/// <summary>Yeni AuditLog oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateAuditLogCommand(CreateAuditLogRequest Request)
    : IRequest<BaseResponse<Guid>>;
