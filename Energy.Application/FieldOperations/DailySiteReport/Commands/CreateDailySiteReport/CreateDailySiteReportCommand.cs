using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReport.Commands.CreateDailySiteReport;

/// <summary>Yeni DailySiteReport oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDailySiteReportCommand(CreateDailySiteReportRequest Request)
    : IRequest<BaseResponse<Guid>>;
