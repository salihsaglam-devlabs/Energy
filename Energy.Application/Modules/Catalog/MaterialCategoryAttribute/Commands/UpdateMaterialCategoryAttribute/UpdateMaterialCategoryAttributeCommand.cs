using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Commands.UpdateMaterialCategoryAttribute;

/// <summary>Var olan MaterialCategoryAttribute kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateMaterialCategoryAttributeCommand(Guid Id, UpdateMaterialCategoryAttributeRequest Request)
    : IRequest<BaseResponse<bool>>;
