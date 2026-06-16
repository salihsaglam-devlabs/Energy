using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAsset.Queries.GetEquipmentAssetLookup;

/// <summary>EquipmentAsset lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetEquipmentAssetLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<EquipmentAssetLookupResponse>>>;
