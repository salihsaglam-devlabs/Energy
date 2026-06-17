using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Requests;
using MediatR;

namespace Energy.Application.Assets.EquipmentAsset.Commands.UpdateEquipmentAsset;

/// <summary>Var olan EquipmentAsset kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateEquipmentAssetCommand(Guid Id, UpdateEquipmentAssetRequest Request)
    : IRequest<BaseResponse<bool>>;
