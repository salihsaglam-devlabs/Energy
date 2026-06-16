using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkillAssignment.Queries.GetEmployeeSkillAssignmentLookup;

/// <summary>
/// <see cref="GetEmployeeSkillAssignmentLookupQuery"/> handler'ı. <see cref="IEmployeeSkillAssignmentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeeSkillAssignmentLookupQueryHandler
    : IRequestHandler<GetEmployeeSkillAssignmentLookupQuery, BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>>
{
    private readonly IEmployeeSkillAssignmentLookupService _lookup;

    public GetEmployeeSkillAssignmentLookupQueryHandler(IEmployeeSkillAssignmentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>> Handle(
        GetEmployeeSkillAssignmentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
