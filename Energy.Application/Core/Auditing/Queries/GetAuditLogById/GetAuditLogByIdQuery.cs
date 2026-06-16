using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;
using Energy.Shared.Identity;
using MediatR;

namespace Energy.Application.Core.Auditing.Queries.GetAuditLogById;

/// <summary>GetAuditLogById</summary>
public sealed record GetAuditLogByIdQuery(long Id)
    : IRequest<BaseResponse<AuditLogResponse>>;
