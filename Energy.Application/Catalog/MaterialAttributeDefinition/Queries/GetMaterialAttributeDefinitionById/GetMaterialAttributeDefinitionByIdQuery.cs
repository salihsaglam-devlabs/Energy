using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeDefinition.Queries.GetMaterialAttributeDefinitionById;

/// <summary>Kimliğe göre MaterialAttributeDefinition detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetMaterialAttributeDefinitionByIdQuery(Guid Id)
    : IRequest<BaseResponse<MaterialAttributeDefinitionDetailResponse>>;
