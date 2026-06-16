using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReport.Queries.GetDailySiteReportById;

/// <summary>Kimliğe göre DailySiteReport detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDailySiteReportByIdQuery(Guid Id)
    : IRequest<BaseResponse<DailySiteReportDetailResponse>>;
