using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReport.Commands.DeleteDailySiteReport;

/// <summary>DailySiteReport kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteDailySiteReportCommand(Guid Id) : IRequest<BaseResponse<bool>>;
