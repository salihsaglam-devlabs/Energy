using Energy.Application.Modules.Contracts.Contract.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.Contract.Commands.DeleteContract;

/// <summary>
/// <see cref="DeleteContractCommand"/> handler'ı. <see cref="IContractService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteContractCommandHandler
    : IRequestHandler<DeleteContractCommand, BaseResponse<bool>>
{
    private readonly IContractService _service;

    public DeleteContractCommandHandler(IContractService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteContractCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
