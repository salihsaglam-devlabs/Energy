using Energy.Application.Modules.Contracts.ContractAmendment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractAmendment.Commands.DeleteContractAmendment;

/// <summary>
/// <see cref="DeleteContractAmendmentCommand"/> handler'ı. <see cref="IContractAmendmentService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteContractAmendmentCommandHandler
    : IRequestHandler<DeleteContractAmendmentCommand, BaseResponse<bool>>
{
    private readonly IContractAmendmentService _service;

    public DeleteContractAmendmentCommandHandler(IContractAmendmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteContractAmendmentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
