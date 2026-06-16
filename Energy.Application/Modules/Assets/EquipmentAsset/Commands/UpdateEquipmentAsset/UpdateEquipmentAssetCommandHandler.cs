using Energy.Application.Modules.Assets.EquipmentAsset.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAsset.Commands.UpdateEquipmentAsset;

/// <summary>
/// <see cref="UpdateEquipmentAssetCommand"/> handler'ı. <see cref="IEquipmentAssetService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateEquipmentAssetCommandHandler
    : IRequestHandler<UpdateEquipmentAssetCommand, BaseResponse<bool>>
{
    private readonly IEquipmentAssetService _service;

    public UpdateEquipmentAssetCommandHandler(IEquipmentAssetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateEquipmentAssetCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
