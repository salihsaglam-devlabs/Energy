using Energy.Application.Modules.Organization.EmployeeSkill.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Queries.GetEmployeeSkillLookup;

/// <summary>
/// <see cref="GetEmployeeSkillLookupQuery"/> handler'ı. <see cref="IEmployeeSkillLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeeSkillLookupQueryHandler
    : IRequestHandler<GetEmployeeSkillLookupQuery, BaseResponse<IReadOnlyList<EmployeeSkillLookupResponse>>>
{
    private readonly IEmployeeSkillLookupService _lookup;

    public GetEmployeeSkillLookupQueryHandler(IEmployeeSkillLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<EmployeeSkillLookupResponse>>> Handle(
        GetEmployeeSkillLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
