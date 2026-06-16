using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepApprover.Queries.GetApprovalStepApproverLookup;

/// <summary>ApprovalStepApprover lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetApprovalStepApproverLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>>;
