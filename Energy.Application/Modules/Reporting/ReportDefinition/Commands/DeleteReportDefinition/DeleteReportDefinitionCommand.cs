using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Reporting.ReportDefinition.Commands.DeleteReportDefinition;

/// <summary>ReportDefinition kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteReportDefinitionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
