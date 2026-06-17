using Energy.Application.Organization.EmployeeSkill.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeeSkill.Queries.GetEmployeeSkillById;

/// <summary>
/// <see cref="GetEmployeeSkillByIdQuery"/> handler'ı. <see cref="IEmployeeSkillService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeeSkillByIdQueryHandler
    : IRequestHandler<GetEmployeeSkillByIdQuery, BaseResponse<EmployeeSkillDetailResponse>>
{
    private readonly IEmployeeSkillService _service;

    public GetEmployeeSkillByIdQueryHandler(IEmployeeSkillService service)
        => _service = service;

    public Task<BaseResponse<EmployeeSkillDetailResponse>> Handle(
        GetEmployeeSkillByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
