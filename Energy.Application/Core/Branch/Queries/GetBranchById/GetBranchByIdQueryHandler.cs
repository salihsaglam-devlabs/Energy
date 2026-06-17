using Energy.Application.Core.Branch.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Responses;
using MediatR;

namespace Energy.Application.Core.Branch.Queries.GetBranchById;

/// <summary>
/// <see cref="GetBranchByIdQuery"/> handler'ı. <see cref="IBranchService"/>'i orkestre eder.
/// </summary>
public sealed class GetBranchByIdQueryHandler
    : IRequestHandler<GetBranchByIdQuery, BaseResponse<BranchDetailResponse>>
{
    private readonly IBranchService _service;

    public GetBranchByIdQueryHandler(IBranchService service)
        => _service = service;

    public Task<BaseResponse<BranchDetailResponse>> Handle(
        GetBranchByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
