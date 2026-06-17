using Energy.Application.Core.Department.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Responses;
using MediatR;

namespace Energy.Application.Core.Department.Queries.GetDepartmentById;

/// <summary>
/// <see cref="GetDepartmentByIdQuery"/> handler'ı. <see cref="IDepartmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetDepartmentByIdQueryHandler
    : IRequestHandler<GetDepartmentByIdQuery, BaseResponse<DepartmentDetailResponse>>
{
    private readonly IDepartmentService _service;

    public GetDepartmentByIdQueryHandler(IDepartmentService service)
        => _service = service;

    public Task<BaseResponse<DepartmentDetailResponse>> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
