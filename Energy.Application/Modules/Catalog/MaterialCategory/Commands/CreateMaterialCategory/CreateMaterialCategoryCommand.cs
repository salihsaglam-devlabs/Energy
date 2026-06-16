using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Commands.CreateMaterialCategory;

/// <summary>Yeni MaterialCategory oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateMaterialCategoryCommand(CreateMaterialCategoryRequest Request)
    : IRequest<BaseResponse<Guid>>;
