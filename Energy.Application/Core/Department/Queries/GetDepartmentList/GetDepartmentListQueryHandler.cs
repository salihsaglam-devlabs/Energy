using Energy.Application.Core.Department.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Responses;
using MediatR;

namespace Energy.Application.Core.Department.Queries.GetDepartmentList;

/// <summary>
/// <see cref="GetDepartmentListQuery"/> handler'ı. <see cref="IDepartmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetDepartmentListQueryHandler
    : IRequestHandler<GetDepartmentListQuery, BaseResponse<PaginatedResponse<DepartmentListResponse>>>
{
    private readonly IDepartmentService _service;

    public GetDepartmentListQueryHandler(IDepartmentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DepartmentListResponse>>> Handle(
        GetDepartmentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
