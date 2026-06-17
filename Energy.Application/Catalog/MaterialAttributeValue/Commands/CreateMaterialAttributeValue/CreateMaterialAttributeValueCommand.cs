using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeValue.Commands.CreateMaterialAttributeValue;

/// <summary>Yeni MaterialAttributeValue oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateMaterialAttributeValueCommand(CreateMaterialAttributeValueRequest Request)
    : IRequest<BaseResponse<Guid>>;
