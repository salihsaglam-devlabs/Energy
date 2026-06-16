using Energy.Application.Organization.Employee.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Responses;
using MediatR;

namespace Energy.Application.Organization.Employee.Queries.GetEmployeeList;

/// <summary>
/// <see cref="GetEmployeeListQuery"/> handler'ı. <see cref="IEmployeeService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeeListQueryHandler
    : IRequestHandler<GetEmployeeListQuery, BaseResponse<PaginatedResponse<EmployeeListResponse>>>
{
    private readonly IEmployeeService _service;

    public GetEmployeeListQueryHandler(IEmployeeService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<EmployeeListResponse>>> Handle(
        GetEmployeeListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
