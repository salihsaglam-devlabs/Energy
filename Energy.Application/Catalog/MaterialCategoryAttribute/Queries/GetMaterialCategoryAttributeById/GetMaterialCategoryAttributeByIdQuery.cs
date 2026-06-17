using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialCategoryAttribute.Queries.GetMaterialCategoryAttributeById;

/// <summary>Kimliğe göre MaterialCategoryAttribute detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetMaterialCategoryAttributeByIdQuery(Guid Id)
    : IRequest<BaseResponse<MaterialCategoryAttributeDetailResponse>>;
