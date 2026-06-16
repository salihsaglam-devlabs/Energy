using Energy.Application.Modules.Inventory.WarehouseLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseLocation.Commands.UpdateWarehouseLocation;

/// <summary>
/// <see cref="UpdateWarehouseLocationCommand"/> handler'ı. <see cref="IWarehouseLocationService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWarehouseLocationCommandHandler
    : IRequestHandler<UpdateWarehouseLocationCommand, BaseResponse<bool>>
{
    private readonly IWarehouseLocationService _service;

    public UpdateWarehouseLocationCommandHandler(IWarehouseLocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWarehouseLocationCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
