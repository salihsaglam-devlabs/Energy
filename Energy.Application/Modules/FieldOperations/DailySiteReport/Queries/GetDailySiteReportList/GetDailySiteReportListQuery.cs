using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReport.Queries.GetDailySiteReportList;

/// <summary>Sayfalanmış DailySiteReport listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDailySiteReportListQuery(GetDailySiteReportListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DailySiteReportListResponse>>>;
