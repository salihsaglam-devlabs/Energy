using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Requests;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.CostCenter.Queries.GetCostCenterList;

/// <summary>Sayfalanmış CostCenter listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetCostCenterListQuery(GetCostCenterListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<CostCenterListResponse>>>;
