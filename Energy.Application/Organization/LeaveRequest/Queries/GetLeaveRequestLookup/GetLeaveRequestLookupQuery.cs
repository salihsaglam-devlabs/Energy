using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;
using MediatR;

namespace Energy.Application.Organization.LeaveRequest.Queries.GetLeaveRequestLookup;

/// <summary>LeaveRequest lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetLeaveRequestLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>>;
