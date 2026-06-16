using Energy.Application.Modules.Inventory.WarehouseTransfer.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseTransfer.Queries.GetWarehouseTransferById;

/// <summary>
/// <see cref="GetWarehouseTransferByIdQuery"/> handler'ı. <see cref="IWarehouseTransferService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseTransferByIdQueryHandler
    : IRequestHandler<GetWarehouseTransferByIdQuery, BaseResponse<WarehouseTransferDetailResponse>>
{
    private readonly IWarehouseTransferService _service;

    public GetWarehouseTransferByIdQueryHandler(IWarehouseTransferService service)
        => _service = service;

    public Task<BaseResponse<WarehouseTransferDetailResponse>> Handle(
        GetWarehouseTransferByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
