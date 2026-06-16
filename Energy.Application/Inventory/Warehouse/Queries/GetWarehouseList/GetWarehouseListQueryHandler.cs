using Energy.Application.Inventory.Warehouse.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Warehouse.Responses;
using MediatR;

namespace Energy.Application.Inventory.Warehouse.Queries.GetWarehouseList;

/// <summary>
/// <see cref="GetWarehouseListQuery"/> handler'ı. <see cref="IWarehouseService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseListQueryHandler
    : IRequestHandler<GetWarehouseListQuery, BaseResponse<PaginatedResponse<WarehouseListResponse>>>
{
    private readonly IWarehouseService _service;

    public GetWarehouseListQueryHandler(IWarehouseService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WarehouseListResponse>>> Handle(
        GetWarehouseListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
