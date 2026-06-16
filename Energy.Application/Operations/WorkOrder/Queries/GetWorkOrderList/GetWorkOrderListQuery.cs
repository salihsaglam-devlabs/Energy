using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrder.Queries.GetWorkOrderList;

/// <summary>Sayfalanmış WorkOrder listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWorkOrderListQuery(GetWorkOrderListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WorkOrderListResponse>>>;
