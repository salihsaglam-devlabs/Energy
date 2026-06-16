using Energy.Application.Modules.Inventory.WarehouseTransfer.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseTransfer.Queries.GetWarehouseTransferLookup;

/// <summary>
/// <see cref="GetWarehouseTransferLookupQuery"/> handler'ı. <see cref="IWarehouseTransferLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseTransferLookupQueryHandler
    : IRequestHandler<GetWarehouseTransferLookupQuery, BaseResponse<IReadOnlyList<WarehouseTransferLookupResponse>>>
{
    private readonly IWarehouseTransferLookupService _lookup;

    public GetWarehouseTransferLookupQueryHandler(IWarehouseTransferLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WarehouseTransferLookupResponse>>> Handle(
        GetWarehouseTransferLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
