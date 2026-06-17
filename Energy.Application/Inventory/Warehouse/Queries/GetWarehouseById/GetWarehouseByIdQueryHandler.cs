using Energy.Application.Inventory.Warehouse.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Warehouse.Responses;
using MediatR;

namespace Energy.Application.Inventory.Warehouse.Queries.GetWarehouseById;

/// <summary>
/// <see cref="GetWarehouseByIdQuery"/> handler'ı. <see cref="IWarehouseService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseByIdQueryHandler
    : IRequestHandler<GetWarehouseByIdQuery, BaseResponse<WarehouseDetailResponse>>
{
    private readonly IWarehouseService _service;

    public GetWarehouseByIdQueryHandler(IWarehouseService service)
        => _service = service;

    public Task<BaseResponse<WarehouseDetailResponse>> Handle(
        GetWarehouseByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
