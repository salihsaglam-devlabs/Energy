using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepDefinition.Queries.GetApprovalStepDefinitionLookup;

/// <summary>ApprovalStepDefinition lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetApprovalStepDefinitionLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>>;
