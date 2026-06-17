using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;
using MediatR;

namespace Energy.Application.Catalog.Brand.Queries.GetBrandById;

/// <summary>Kimliğe göre Brand detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetBrandByIdQuery(Guid Id)
    : IRequest<BaseResponse<BrandDetailResponse>>;
