using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Requests;
using MediatR;

namespace Energy.Application.Catalog.Material.Commands.UpdateMaterial;

/// <summary>Var olan Material kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateMaterialCommand(Guid Id, UpdateMaterialRequest Request)
    : IRequest<BaseResponse<bool>>;
