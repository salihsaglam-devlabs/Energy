using Energy.Application.Modules.Contracts.ContractLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractLine.Commands.DeleteContractLine;

/// <summary>
/// <see cref="DeleteContractLineCommand"/> handler'ı. <see cref="IContractLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteContractLineCommandHandler
    : IRequestHandler<DeleteContractLineCommand, BaseResponse<bool>>
{
    private readonly IContractLineService _service;

    public DeleteContractLineCommandHandler(IContractLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteContractLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
