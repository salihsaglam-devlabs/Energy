using Energy.Application.Modules.Inventory.WarehouseTransfer.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseTransfer.Queries.GetWarehouseTransferList;

/// <summary>
/// <see cref="GetWarehouseTransferListQuery"/> handler'ı. <see cref="IWarehouseTransferService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseTransferListQueryHandler
    : IRequestHandler<GetWarehouseTransferListQuery, BaseResponse<PaginatedResponse<WarehouseTransferListResponse>>>
{
    private readonly IWarehouseTransferService _service;

    public GetWarehouseTransferListQueryHandler(IWarehouseTransferService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WarehouseTransferListResponse>>> Handle(
        GetWarehouseTransferListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
