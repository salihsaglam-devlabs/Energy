using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequestApprover.Queries.GetApprovalRequestApproverLookup;

/// <summary>ApprovalRequestApprover lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetApprovalRequestApproverLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>>;
