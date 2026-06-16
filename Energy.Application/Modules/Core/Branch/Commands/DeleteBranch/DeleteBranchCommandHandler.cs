using Energy.Application.Modules.Core.Branch.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Branch.Commands.DeleteBranch;

/// <summary>
/// <see cref="DeleteBranchCommand"/> handler'ı. <see cref="IBranchService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteBranchCommandHandler
    : IRequestHandler<DeleteBranchCommand, BaseResponse<bool>>
{
    private readonly IBranchService _service;

    public DeleteBranchCommandHandler(IBranchService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteBranchCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
