using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportWorker.Commands.UpdateDailySiteReportWorker;

/// <summary>Var olan DailySiteReportWorker kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDailySiteReportWorkerCommand(Guid Id, UpdateDailySiteReportWorkerRequest Request)
    : IRequest<BaseResponse<bool>>;
