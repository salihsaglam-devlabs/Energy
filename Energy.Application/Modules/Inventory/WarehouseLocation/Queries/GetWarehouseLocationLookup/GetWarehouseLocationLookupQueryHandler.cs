using Energy.Application.Modules.Inventory.WarehouseLocation.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseLocation.Queries.GetWarehouseLocationLookup;

/// <summary>
/// <see cref="GetWarehouseLocationLookupQuery"/> handler'ı. <see cref="IWarehouseLocationLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseLocationLookupQueryHandler
    : IRequestHandler<GetWarehouseLocationLookupQuery, BaseResponse<IReadOnlyList<WarehouseLocationLookupResponse>>>
{
    private readonly IWarehouseLocationLookupService _lookup;

    public GetWarehouseLocationLookupQueryHandler(IWarehouseLocationLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WarehouseLocationLookupResponse>>> Handle(
        GetWarehouseLocationLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
