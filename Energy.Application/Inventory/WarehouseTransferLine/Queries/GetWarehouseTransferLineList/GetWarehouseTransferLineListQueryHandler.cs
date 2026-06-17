using Energy.Application.Inventory.WarehouseTransferLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransferLine.Queries.GetWarehouseTransferLineList;

/// <summary>
/// <see cref="GetWarehouseTransferLineListQuery"/> handler'ı. <see cref="IWarehouseTransferLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetWarehouseTransferLineListQueryHandler
    : IRequestHandler<GetWarehouseTransferLineListQuery, BaseResponse<PaginatedResponse<WarehouseTransferLineListResponse>>>
{
    private readonly IWarehouseTransferLineService _service;

    public GetWarehouseTransferLineListQueryHandler(IWarehouseTransferLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WarehouseTransferLineListResponse>>> Handle(
        GetWarehouseTransferLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
