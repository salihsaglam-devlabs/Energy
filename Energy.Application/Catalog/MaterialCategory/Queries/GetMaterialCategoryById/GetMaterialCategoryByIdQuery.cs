using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialCategory.Queries.GetMaterialCategoryById;

/// <summary>Kimliğe göre MaterialCategory detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetMaterialCategoryByIdQuery(Guid Id)
    : IRequest<BaseResponse<MaterialCategoryDetailResponse>>;
