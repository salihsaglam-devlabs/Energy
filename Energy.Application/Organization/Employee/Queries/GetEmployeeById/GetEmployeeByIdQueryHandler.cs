using Energy.Application.Organization.Employee.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Responses;
using MediatR;

namespace Energy.Application.Organization.Employee.Queries.GetEmployeeById;

/// <summary>
/// <see cref="GetEmployeeByIdQuery"/> handler'ı. <see cref="IEmployeeService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeeByIdQueryHandler
    : IRequestHandler<GetEmployeeByIdQuery, BaseResponse<EmployeeDetailResponse>>
{
    private readonly IEmployeeService _service;

    public GetEmployeeByIdQueryHandler(IEmployeeService service)
        => _service = service;

    public Task<BaseResponse<EmployeeDetailResponse>> Handle(
        GetEmployeeByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
