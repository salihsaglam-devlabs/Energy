using Energy.Application.Core.Branch.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.Branch.Commands.CreateBranch;

/// <summary>
/// <see cref="CreateBranchCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IBranchService"/>'i orkestre eder.
/// </summary>
public sealed class CreateBranchCommandHandler
    : IRequestHandler<CreateBranchCommand, BaseResponse<Guid>>
{
    private readonly IBranchService _service;

    public CreateBranchCommandHandler(IBranchService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateBranchCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
