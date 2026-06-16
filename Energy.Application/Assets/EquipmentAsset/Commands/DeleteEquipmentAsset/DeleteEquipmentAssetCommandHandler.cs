using Energy.Application.Assets.EquipmentAsset.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAsset.Commands.DeleteEquipmentAsset;

/// <summary>
/// <see cref="DeleteEquipmentAssetCommand"/> handler'ı. <see cref="IEquipmentAssetService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteEquipmentAssetCommandHandler
    : IRequestHandler<DeleteEquipmentAssetCommand, BaseResponse<bool>>
{
    private readonly IEquipmentAssetService _service;

    public DeleteEquipmentAssetCommandHandler(IEquipmentAssetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteEquipmentAssetCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
