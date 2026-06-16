using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitConversion.Queries.GetUnitConversionById;

/// <summary>Kimliğe göre UnitConversion detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetUnitConversionByIdQuery(Guid Id)
    : IRequest<BaseResponse<UnitConversionDetailResponse>>;
