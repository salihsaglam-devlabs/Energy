using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAsset.Queries.GetEquipmentAssetList;

/// <summary>Sayfalanmış EquipmentAsset listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetEquipmentAssetListQuery(GetEquipmentAssetListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<EquipmentAssetListResponse>>>;
