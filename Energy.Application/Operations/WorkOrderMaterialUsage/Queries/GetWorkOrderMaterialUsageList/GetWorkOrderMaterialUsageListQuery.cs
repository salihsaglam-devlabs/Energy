using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Queries.GetWorkOrderMaterialUsageList;

/// <summary>Sayfalanmış WorkOrderMaterialUsage listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWorkOrderMaterialUsageListQuery(GetWorkOrderMaterialUsageListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>>;
