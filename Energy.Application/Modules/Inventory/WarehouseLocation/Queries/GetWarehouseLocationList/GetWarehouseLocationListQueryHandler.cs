using Energy.Application.Modules.Inventory.WarehouseLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseLocation.Queries.GetWarehouseLocationList;

/// <summary>
/// <see cref="GetWarehouseLocationListQuery"/> handler'ı. <see cref="IWarehouseLocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseLocationListQueryHandler
    : IRequestHandler<GetWarehouseLocationListQuery, BaseResponse<PaginatedResponse<WarehouseLocationListResponse>>>
{
    private readonly IWarehouseLocationService _service;

    public GetWarehouseLocationListQueryHandler(IWarehouseLocationService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WarehouseLocationListResponse>>> Handle(
        GetWarehouseLocationListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
