using Energy.Application.Modules.Inventory.WarehouseTransferLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseTransferLine.Queries.GetWarehouseTransferLineById;

/// <summary>
/// <see cref="GetWarehouseTransferLineByIdQuery"/> handler'ı. <see cref="IWarehouseTransferLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseTransferLineByIdQueryHandler
    : IRequestHandler<GetWarehouseTransferLineByIdQuery, BaseResponse<WarehouseTransferLineDetailResponse>>
{
    private readonly IWarehouseTransferLineService _service;

    public GetWarehouseTransferLineByIdQueryHandler(IWarehouseTransferLineService service)
        => _service = service;

    public Task<BaseResponse<WarehouseTransferLineDetailResponse>> Handle(
        GetWarehouseTransferLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
