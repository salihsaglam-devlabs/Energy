using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Commands.DeleteMaterialCategoryAttribute;

/// <summary>MaterialCategoryAttribute kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteMaterialCategoryAttributeCommand(Guid Id) : IRequest<BaseResponse<bool>>;
