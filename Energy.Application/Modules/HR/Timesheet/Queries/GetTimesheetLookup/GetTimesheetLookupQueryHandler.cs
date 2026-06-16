using Energy.Application.Modules.HR.Timesheet.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.Timesheet.Queries.GetTimesheetLookup;

/// <summary>
/// <see cref="GetTimesheetLookupQuery"/> handler'ı. <see cref="ITimesheetLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetTimesheetLookupQueryHandler
    : IRequestHandler<GetTimesheetLookupQuery, BaseResponse<IReadOnlyList<TimesheetLookupResponse>>>
{
    private readonly ITimesheetLookupService _lookup;

    public GetTimesheetLookupQueryHandler(ITimesheetLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<TimesheetLookupResponse>>> Handle(
        GetTimesheetLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
