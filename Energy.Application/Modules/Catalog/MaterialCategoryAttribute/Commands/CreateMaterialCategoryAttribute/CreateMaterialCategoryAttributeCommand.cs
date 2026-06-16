using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Commands.CreateMaterialCategoryAttribute;

/// <summary>Yeni MaterialCategoryAttribute oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateMaterialCategoryAttributeCommand(CreateMaterialCategoryAttributeRequest Request)
    : IRequest<BaseResponse<Guid>>;
