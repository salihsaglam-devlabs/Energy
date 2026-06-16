using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReport.Commands.UpdateDailySiteReport;

/// <summary>Var olan DailySiteReport kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDailySiteReportCommand(Guid Id, UpdateDailySiteReportRequest Request)
    : IRequest<BaseResponse<bool>>;
