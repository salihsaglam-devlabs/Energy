using Energy.Application.Modules.Inventory.Warehouse.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Warehouse.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.Warehouse.Queries.GetWarehouseLookup;

/// <summary>
/// <see cref="GetWarehouseLookupQuery"/> handler'ı. <see cref="IWarehouseLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseLookupQueryHandler
    : IRequestHandler<GetWarehouseLookupQuery, BaseResponse<IReadOnlyList<WarehouseLookupResponse>>>
{
    private readonly IWarehouseLookupService _lookup;

    public GetWarehouseLookupQueryHandler(IWarehouseLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WarehouseLookupResponse>>> Handle(
        GetWarehouseLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
