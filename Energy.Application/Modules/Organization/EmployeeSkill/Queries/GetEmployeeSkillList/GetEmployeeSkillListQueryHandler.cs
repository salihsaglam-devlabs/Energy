using Energy.Application.Modules.Organization.EmployeeSkill.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Queries.GetEmployeeSkillList;

/// <summary>
/// <see cref="GetEmployeeSkillListQuery"/> handler'ı. <see cref="IEmployeeSkillService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeeSkillListQueryHandler
    : IRequestHandler<GetEmployeeSkillListQuery, BaseResponse<PaginatedResponse<EmployeeSkillListResponse>>>
{
    private readonly IEmployeeSkillService _service;

    public GetEmployeeSkillListQueryHandler(IEmployeeSkillService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<EmployeeSkillListResponse>>> Handle(
        GetEmployeeSkillListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
