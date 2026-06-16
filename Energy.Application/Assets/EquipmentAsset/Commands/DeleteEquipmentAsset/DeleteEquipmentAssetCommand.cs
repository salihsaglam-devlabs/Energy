using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAsset.Commands.DeleteEquipmentAsset;

/// <summary>EquipmentAsset kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteEquipmentAssetCommand(Guid Id) : IRequest<BaseResponse<bool>>;
