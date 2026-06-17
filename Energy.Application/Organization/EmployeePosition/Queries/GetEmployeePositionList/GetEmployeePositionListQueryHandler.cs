using Energy.Application.Organization.EmployeePosition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeePosition.Queries.GetEmployeePositionList;

/// <summary>
/// <see cref="GetEmployeePositionListQuery"/> handler'ı. <see cref="IEmployeePositionService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeePositionListQueryHandler
    : IRequestHandler<GetEmployeePositionListQuery, BaseResponse<PaginatedResponse<EmployeePositionListResponse>>>
{
    private readonly IEmployeePositionService _service;

    public GetEmployeePositionListQueryHandler(IEmployeePositionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<EmployeePositionListResponse>>> Handle(
        GetEmployeePositionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
