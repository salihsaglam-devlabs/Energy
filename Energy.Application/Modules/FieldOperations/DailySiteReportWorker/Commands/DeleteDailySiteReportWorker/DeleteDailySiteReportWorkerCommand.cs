using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Commands.DeleteDailySiteReportWorker;

/// <summary>DailySiteReportWorker kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteDailySiteReportWorkerCommand(Guid Id) : IRequest<BaseResponse<bool>>;
