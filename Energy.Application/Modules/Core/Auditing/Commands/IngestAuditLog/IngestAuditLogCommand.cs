using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;
using Energy.Shared.Identity;
using MediatR;

namespace Energy.Application.Modules.Core.Auditing.Commands.IngestAuditLog;

/// <summary>IngestAuditLog</summary>
public sealed record IngestAuditLogCommand(CreateAuditLogRequest Request, string? IpAddress)
    : IRequest<BaseResponse<bool>>;
