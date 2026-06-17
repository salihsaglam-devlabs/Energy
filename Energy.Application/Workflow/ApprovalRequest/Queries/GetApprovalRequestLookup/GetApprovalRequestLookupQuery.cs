using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequest.Queries.GetApprovalRequestLookup;

/// <summary>ApprovalRequest lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetApprovalRequestLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ApprovalRequestLookupResponse>>>;
