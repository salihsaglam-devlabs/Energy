using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAsset.Queries.GetEquipmentAssetById;

/// <summary>Kimliğe göre EquipmentAsset detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetEquipmentAssetByIdQuery(Guid Id)
    : IRequest<BaseResponse<EquipmentAssetDetailResponse>>;
