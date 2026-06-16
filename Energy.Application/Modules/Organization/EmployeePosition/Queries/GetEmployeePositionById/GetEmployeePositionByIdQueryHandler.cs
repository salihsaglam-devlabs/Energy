using Energy.Application.Modules.Organization.EmployeePosition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeePosition.Queries.GetEmployeePositionById;

/// <summary>
/// <see cref="GetEmployeePositionByIdQuery"/> handler'ı. <see cref="IEmployeePositionService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeePositionByIdQueryHandler
    : IRequestHandler<GetEmployeePositionByIdQuery, BaseResponse<EmployeePositionDetailResponse>>
{
    private readonly IEmployeePositionService _service;

    public GetEmployeePositionByIdQueryHandler(IEmployeePositionService service)
        => _service = service;

    public Task<BaseResponse<EmployeePositionDetailResponse>> Handle(
        GetEmployeePositionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
