using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeOption.Queries.GetMaterialAttributeOptionById;

/// <summary>Kimliğe göre MaterialAttributeOption detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetMaterialAttributeOptionByIdQuery(Guid Id)
    : IRequest<BaseResponse<MaterialAttributeOptionDetailResponse>>;
