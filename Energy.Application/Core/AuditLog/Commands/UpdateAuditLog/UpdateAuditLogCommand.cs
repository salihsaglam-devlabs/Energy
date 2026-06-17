using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Requests;
using MediatR;

namespace Energy.Application.Core.AuditLog.Commands.UpdateAuditLog;

/// <summary>Var olan AuditLog kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateAuditLogCommand(Guid Id, UpdateAuditLogRequest Request)
    : IRequest<BaseResponse<bool>>;
