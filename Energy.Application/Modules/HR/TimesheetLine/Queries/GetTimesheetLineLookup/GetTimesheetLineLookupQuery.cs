using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.TimesheetLine.Queries.GetTimesheetLineLookup;

/// <summary>TimesheetLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetTimesheetLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<TimesheetLineLookupResponse>>>;
