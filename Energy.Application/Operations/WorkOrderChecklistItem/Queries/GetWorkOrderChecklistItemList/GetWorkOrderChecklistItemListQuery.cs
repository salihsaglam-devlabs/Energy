using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklistItem.Queries.GetWorkOrderChecklistItemList;

/// <summary>Sayfalanmış WorkOrderChecklistItem listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWorkOrderChecklistItemListQuery(GetWorkOrderChecklistItemListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WorkOrderChecklistItemListResponse>>>;
