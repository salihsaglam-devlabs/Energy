using Energy.Application.Inventory.WarehouseLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseLocation.Queries.GetWarehouseLocationById;

/// <summary>
/// <see cref="GetWarehouseLocationByIdQuery"/> handler'ı. <see cref="IWarehouseLocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseLocationByIdQueryHandler
    : IRequestHandler<GetWarehouseLocationByIdQuery, BaseResponse<WarehouseLocationDetailResponse>>
{
    private readonly IWarehouseLocationService _service;

    public GetWarehouseLocationByIdQueryHandler(IWarehouseLocationService service)
        => _service = service;

    public Task<BaseResponse<WarehouseLocationDetailResponse>> Handle(
        GetWarehouseLocationByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
