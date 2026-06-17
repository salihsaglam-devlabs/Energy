using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequestStep.Queries.GetApprovalRequestStepLookup;

/// <summary>ApprovalRequestStep lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetApprovalRequestStepLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>>;
