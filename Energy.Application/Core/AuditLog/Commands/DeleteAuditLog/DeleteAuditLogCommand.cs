using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.AuditLog.Commands.DeleteAuditLog;

/// <summary>AuditLog kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteAuditLogCommand(Guid Id) : IRequest<BaseResponse<bool>>;
