using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Requests;
using MediatR;

namespace Energy.Application.Modules.Reporting.ReportDefinition.Commands.CreateReportDefinition;

/// <summary>Yeni ReportDefinition oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateReportDefinitionCommand(CreateReportDefinitionRequest Request)
    : IRequest<BaseResponse<Guid>>;
