using Energy.Application.Assets.EquipmentAsset.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAsset.Queries.GetEquipmentAssetById;

/// <summary>
/// <see cref="GetEquipmentAssetByIdQuery"/> handler'ı. <see cref="IEquipmentAssetService"/>'i orkestre eder.
/// </summary>
public sealed class GetEquipmentAssetByIdQueryHandler
    : IRequestHandler<GetEquipmentAssetByIdQuery, BaseResponse<EquipmentAssetDetailResponse>>
{
    private readonly IEquipmentAssetService _service;

    public GetEquipmentAssetByIdQueryHandler(IEquipmentAssetService service)
        => _service = service;

    public Task<BaseResponse<EquipmentAssetDetailResponse>> Handle(
        GetEquipmentAssetByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
