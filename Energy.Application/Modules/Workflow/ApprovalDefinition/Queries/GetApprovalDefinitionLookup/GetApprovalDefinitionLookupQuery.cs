using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinition.Queries.GetApprovalDefinitionLookup;

/// <summary>ApprovalDefinition lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetApprovalDefinitionLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ApprovalDefinitionLookupResponse>>>;
