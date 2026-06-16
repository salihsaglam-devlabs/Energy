using Energy.Application.Modules.Finance.CostCenter.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.CostCenter.Queries.GetCostCenterLookup;

/// <summary>
/// <see cref="GetCostCenterLookupQuery"/> handler'ı. <see cref="ICostCenterLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetCostCenterLookupQueryHandler
    : IRequestHandler<GetCostCenterLookupQuery, BaseResponse<IReadOnlyList<CostCenterLookupResponse>>>
{
    private readonly ICostCenterLookupService _lookup;

    public GetCostCenterLookupQueryHandler(ICostCenterLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<CostCenterLookupResponse>>> Handle(
        GetCostCenterLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
