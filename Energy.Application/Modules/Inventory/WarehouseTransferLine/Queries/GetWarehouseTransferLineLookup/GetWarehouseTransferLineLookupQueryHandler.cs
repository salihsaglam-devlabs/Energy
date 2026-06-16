using Energy.Application.Modules.Inventory.WarehouseTransferLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseTransferLine.Queries.GetWarehouseTransferLineLookup;

/// <summary>
/// <see cref="GetWarehouseTransferLineLookupQuery"/> handler'ı. <see cref="IWarehouseTransferLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseTransferLineLookupQueryHandler
    : IRequestHandler<GetWarehouseTransferLineLookupQuery, BaseResponse<IReadOnlyList<WarehouseTransferLineLookupResponse>>>
{
    private readonly IWarehouseTransferLineLookupService _lookup;

    public GetWarehouseTransferLineLookupQueryHandler(IWarehouseTransferLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WarehouseTransferLineLookupResponse>>> Handle(
        GetWarehouseTransferLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
