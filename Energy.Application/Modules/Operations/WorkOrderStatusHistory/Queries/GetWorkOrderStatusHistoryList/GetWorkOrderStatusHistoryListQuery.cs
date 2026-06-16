using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderStatusHistory.Queries.GetWorkOrderStatusHistoryList;

/// <summary>Sayfalanmış WorkOrderStatusHistory listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWorkOrderStatusHistoryListQuery(GetWorkOrderStatusHistoryListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>>;
