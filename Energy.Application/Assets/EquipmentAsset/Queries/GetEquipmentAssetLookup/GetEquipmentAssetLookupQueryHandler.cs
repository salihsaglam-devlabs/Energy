using Energy.Application.Assets.EquipmentAsset.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAsset.Queries.GetEquipmentAssetLookup;

/// <summary>
/// <see cref="GetEquipmentAssetLookupQuery"/> handler'ı. <see cref="IEquipmentAssetLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetEquipmentAssetLookupQueryHandler
    : IRequestHandler<GetEquipmentAssetLookupQuery, BaseResponse<IReadOnlyList<EquipmentAssetLookupResponse>>>
{
    private readonly IEquipmentAssetLookupService _lookup;

    public GetEquipmentAssetLookupQueryHandler(IEquipmentAssetLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<EquipmentAssetLookupResponse>>> Handle(
        GetEquipmentAssetLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
