using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeValue.Queries.GetMaterialAttributeValueById;

/// <summary>Kimliğe göre MaterialAttributeValue detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetMaterialAttributeValueByIdQuery(Guid Id)
    : IRequest<BaseResponse<MaterialAttributeValueDetailResponse>>;
