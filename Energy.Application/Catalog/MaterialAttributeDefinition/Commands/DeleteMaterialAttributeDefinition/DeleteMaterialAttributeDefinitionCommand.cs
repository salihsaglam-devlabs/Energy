using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeDefinition.Commands.DeleteMaterialAttributeDefinition;

/// <summary>MaterialAttributeDefinition kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteMaterialAttributeDefinitionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
