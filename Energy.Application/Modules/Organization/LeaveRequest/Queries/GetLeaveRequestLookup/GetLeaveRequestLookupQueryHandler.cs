using Energy.Application.Modules.Organization.LeaveRequest.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.LeaveRequest.Queries.GetLeaveRequestLookup;

/// <summary>
/// <see cref="GetLeaveRequestLookupQuery"/> handler'ı. <see cref="ILeaveRequestLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetLeaveRequestLookupQueryHandler
    : IRequestHandler<GetLeaveRequestLookupQuery, BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>>
{
    private readonly ILeaveRequestLookupService _lookup;

    public GetLeaveRequestLookupQueryHandler(ILeaveRequestLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>> Handle(
        GetLeaveRequestLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
