using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderType.Queries.GetWorkOrderTypeList;

/// <summary>Sayfalanmış WorkOrderType listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWorkOrderTypeListQuery(GetWorkOrderTypeListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WorkOrderTypeListResponse>>>;
