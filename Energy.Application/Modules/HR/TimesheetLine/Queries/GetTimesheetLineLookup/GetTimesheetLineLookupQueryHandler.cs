using Energy.Application.Modules.HR.TimesheetLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.TimesheetLine.Queries.GetTimesheetLineLookup;

/// <summary>
/// <see cref="GetTimesheetLineLookupQuery"/> handler'ı. <see cref="ITimesheetLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetTimesheetLineLookupQueryHandler
    : IRequestHandler<GetTimesheetLineLookupQuery, BaseResponse<IReadOnlyList<TimesheetLineLookupResponse>>>
{
    private readonly ITimesheetLineLookupService _lookup;

    public GetTimesheetLineLookupQueryHandler(ITimesheetLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<TimesheetLineLookupResponse>>> Handle(
        GetTimesheetLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
