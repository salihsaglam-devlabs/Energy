using Energy.Application.Modules.Core.Branch.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Branch.Commands.UpdateBranch;

/// <summary>
/// <see cref="UpdateBranchCommand"/> handler'ı. <see cref="IBranchService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateBranchCommandHandler
    : IRequestHandler<UpdateBranchCommand, BaseResponse<bool>>
{
    private readonly IBranchService _service;

    public UpdateBranchCommandHandler(IBranchService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateBranchCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
