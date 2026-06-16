using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Commands.UpdateMaterialCategory;

/// <summary>Var olan MaterialCategory kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateMaterialCategoryCommand(Guid Id, UpdateMaterialCategoryRequest Request)
    : IRequest<BaseResponse<bool>>;
