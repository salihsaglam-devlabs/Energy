using Energy.Application.Assets.EquipmentAsset.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAsset.Commands.CreateEquipmentAsset;

/// <summary>
/// <see cref="CreateEquipmentAssetCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IEquipmentAssetService"/>'i orkestre eder.
/// </summary>
public sealed class CreateEquipmentAssetCommandHandler
    : IRequestHandler<CreateEquipmentAssetCommand, BaseResponse<Guid>>
{
    private readonly IEquipmentAssetService _service;

    public CreateEquipmentAssetCommandHandler(IEquipmentAssetService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateEquipmentAssetCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
