using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Requests;
using MediatR;

namespace Energy.Application.Modules.Reporting.ReportDefinition.Commands.UpdateReportDefinition;

/// <summary>Var olan ReportDefinition kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateReportDefinitionCommand(Guid Id, UpdateReportDefinitionRequest Request)
    : IRequest<BaseResponse<bool>>;
