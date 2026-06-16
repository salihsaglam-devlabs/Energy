using Energy.Application.Modules.Finance.CostCenter.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.CostCenter.Queries.GetCostCenterById;

/// <summary>
/// <see cref="GetCostCenterByIdQuery"/> handler'ı. <see cref="ICostCenterService"/>'i orkestre eder.
/// </summary>
public sealed class GetCostCenterByIdQueryHandler
    : IRequestHandler<GetCostCenterByIdQuery, BaseResponse<CostCenterDetailResponse>>
{
    private readonly ICostCenterService _service;

    public GetCostCenterByIdQueryHandler(ICostCenterService service)
        => _service = service;

    public Task<BaseResponse<CostCenterDetailResponse>> Handle(
        GetCostCenterByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
