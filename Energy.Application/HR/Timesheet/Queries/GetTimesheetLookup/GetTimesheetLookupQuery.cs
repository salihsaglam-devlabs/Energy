using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;
using MediatR;

namespace Energy.Application.HR.Timesheet.Queries.GetTimesheetLookup;

/// <summary>Timesheet lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetTimesheetLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<TimesheetLookupResponse>>>;
