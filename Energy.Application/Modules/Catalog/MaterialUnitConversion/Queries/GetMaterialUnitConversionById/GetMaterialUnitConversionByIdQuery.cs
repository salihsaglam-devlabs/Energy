using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialUnitConversion.Queries.GetMaterialUnitConversionById;

/// <summary>Kimliğe göre MaterialUnitConversion detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetMaterialUnitConversionByIdQuery(Guid Id)
    : IRequest<BaseResponse<MaterialUnitConversionDetailResponse>>;
