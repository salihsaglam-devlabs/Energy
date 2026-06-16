using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Requests;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeDefinition.Commands.CreateMaterialAttributeDefinition;

/// <summary>Yeni MaterialAttributeDefinition oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateMaterialAttributeDefinitionCommand(CreateMaterialAttributeDefinitionRequest Request)
    : IRequest<BaseResponse<Guid>>;
