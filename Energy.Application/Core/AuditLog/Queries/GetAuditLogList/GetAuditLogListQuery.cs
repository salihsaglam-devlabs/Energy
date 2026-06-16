using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Requests;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;
using MediatR;

namespace Energy.Application.Core.AuditLog.Queries.GetAuditLogList;

/// <summary>Sayfalanmış AuditLog listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetAuditLogListQuery(GetAuditLogListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<AuditLogListResponse>>>;
