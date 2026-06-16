using Energy.Application.Modules.Assets.EquipmentAsset.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAsset.Queries.GetEquipmentAssetList;

/// <summary>
/// <see cref="GetEquipmentAssetListQuery"/> handler'ı. <see cref="IEquipmentAssetService"/>'i orkestre eder.
/// </summary>
public sealed class GetEquipmentAssetListQueryHandler
    : IRequestHandler<GetEquipmentAssetListQuery, BaseResponse<PaginatedResponse<EquipmentAssetListResponse>>>
{
    private readonly IEquipmentAssetService _service;

    public GetEquipmentAssetListQueryHandler(IEquipmentAssetService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<EquipmentAssetListResponse>>> Handle(
        GetEquipmentAssetListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
