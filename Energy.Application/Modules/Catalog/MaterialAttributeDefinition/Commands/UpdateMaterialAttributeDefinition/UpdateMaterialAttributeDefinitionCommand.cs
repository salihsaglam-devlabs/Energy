using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Requests;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Commands.UpdateMaterialAttributeDefinition;

/// <summary>Var olan MaterialAttributeDefinition kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateMaterialAttributeDefinitionCommand(Guid Id, UpdateMaterialAttributeDefinitionRequest Request)
    : IRequest<BaseResponse<bool>>;
