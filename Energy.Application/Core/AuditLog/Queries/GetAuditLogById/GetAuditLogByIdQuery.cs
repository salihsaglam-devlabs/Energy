using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;
using MediatR;

namespace Energy.Application.Core.AuditLog.Queries.GetAuditLogById;

/// <summary>Kimliğe göre AuditLog detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetAuditLogByIdQuery(Guid Id)
    : IRequest<BaseResponse<AuditLogDetailResponse>>;
