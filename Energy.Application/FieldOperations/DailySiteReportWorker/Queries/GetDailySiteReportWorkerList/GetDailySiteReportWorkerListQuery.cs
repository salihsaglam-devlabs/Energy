using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportWorker.Queries.GetDailySiteReportWorkerList;

/// <summary>Sayfalanmış DailySiteReportWorker listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDailySiteReportWorkerListQuery(GetDailySiteReportWorkerListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>>;
