using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Responses;
using MediatR;

namespace Energy.Application.Catalog.Material.Queries.GetMaterialById;

/// <summary>Kimliğe göre Material detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetMaterialByIdQuery(Guid Id)
    : IRequest<BaseResponse<MaterialDetailResponse>>;
