using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Requests;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAsset.Commands.CreateEquipmentAsset;

/// <summary>Yeni EquipmentAsset oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateEquipmentAssetCommand(CreateEquipmentAssetRequest Request)
    : IRequest<BaseResponse<Guid>>;
