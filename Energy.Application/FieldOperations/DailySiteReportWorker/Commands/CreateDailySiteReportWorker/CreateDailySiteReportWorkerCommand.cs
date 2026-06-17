using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportWorker.Commands.CreateDailySiteReportWorker;

/// <summary>Yeni DailySiteReportWorker oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDailySiteReportWorkerCommand(CreateDailySiteReportWorkerRequest Request)
    : IRequest<BaseResponse<Guid>>;
