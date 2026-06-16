using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Commands.DeleteMaterialCategory;

/// <summary>MaterialCategory kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteMaterialCategoryCommand(Guid Id) : IRequest<BaseResponse<bool>>;
