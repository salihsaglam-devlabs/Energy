using Energy.Application.Core.Department.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Responses;
using MediatR;

namespace Energy.Application.Core.Department.Queries.GetDepartmentLookup;

/// <summary>
/// <see cref="GetDepartmentLookupQuery"/> handler'ı. <see cref="IDepartmentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDepartmentLookupQueryHandler
    : IRequestHandler<GetDepartmentLookupQuery, BaseResponse<IReadOnlyList<DepartmentLookupResponse>>>
{
    private readonly IDepartmentLookupService _lookup;

    public GetDepartmentLookupQueryHandler(IDepartmentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DepartmentLookupResponse>>> Handle(
        GetDepartmentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
