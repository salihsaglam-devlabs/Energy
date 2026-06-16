using Energy.Application.Modules.Organization.Employee.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.Employee.Queries.GetEmployeeLookup;

/// <summary>
/// <see cref="GetEmployeeLookupQuery"/> handler'ı. <see cref="IEmployeeLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeeLookupQueryHandler
    : IRequestHandler<GetEmployeeLookupQuery, BaseResponse<IReadOnlyList<EmployeeLookupResponse>>>
{
    private readonly IEmployeeLookupService _lookup;

    public GetEmployeeLookupQueryHandler(IEmployeeLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<EmployeeLookupResponse>>> Handle(
        GetEmployeeLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
