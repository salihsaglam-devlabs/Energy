using Energy.Application.Organization.EmployeePosition.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeePosition.Queries.GetEmployeePositionLookup;

/// <summary>
/// <see cref="GetEmployeePositionLookupQuery"/> handler'ı. <see cref="IEmployeePositionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeePositionLookupQueryHandler
    : IRequestHandler<GetEmployeePositionLookupQuery, BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>>
{
    private readonly IEmployeePositionLookupService _lookup;

    public GetEmployeePositionLookupQueryHandler(IEmployeePositionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>> Handle(
        GetEmployeePositionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
