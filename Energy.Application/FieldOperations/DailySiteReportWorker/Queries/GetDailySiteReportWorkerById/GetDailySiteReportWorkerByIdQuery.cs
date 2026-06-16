using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportWorker.Queries.GetDailySiteReportWorkerById;

/// <summary>Kimliğe göre DailySiteReportWorker detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDailySiteReportWorkerByIdQuery(Guid Id)
    : IRequest<BaseResponse<DailySiteReportWorkerDetailResponse>>;
