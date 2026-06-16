using Energy.Application.Modules.Finance.CostCenter.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.CostCenter.Queries.GetCostCenterList;

/// <summary>
/// <see cref="GetCostCenterListQuery"/> handler'ı. <see cref="ICostCenterService"/>'i orkestre eder.
/// </summary>
public sealed class GetCostCenterListQueryHandler
    : IRequestHandler<GetCostCenterListQuery, BaseResponse<PaginatedResponse<CostCenterListResponse>>>
{
    private readonly ICostCenterService _service;

    public GetCostCenterListQueryHandler(ICostCenterService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<CostCenterListResponse>>> Handle(
        GetCostCenterListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
