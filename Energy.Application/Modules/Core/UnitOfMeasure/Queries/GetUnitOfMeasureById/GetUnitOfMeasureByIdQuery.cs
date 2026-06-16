using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitOfMeasure.Queries.GetUnitOfMeasureById;

/// <summary>Kimliğe göre UnitOfMeasure detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetUnitOfMeasureByIdQuery(Guid Id)
    : IRequest<BaseResponse<UnitOfMeasureDetailResponse>>;
