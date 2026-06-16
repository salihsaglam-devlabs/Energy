using Energy.Application.Modules.Core.Branch.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Branch.Queries.GetBranchList;

/// <summary>
/// <see cref="GetBranchListQuery"/> handler'ı. <see cref="IBranchService"/>'i orkestre eder.
/// </summary>
public sealed class GetBranchListQueryHandler
    : IRequestHandler<GetBranchListQuery, BaseResponse<PaginatedResponse<BranchListResponse>>>
{
    private readonly IBranchService _service;

    public GetBranchListQueryHandler(IBranchService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<BranchListResponse>>> Handle(
        GetBranchListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
