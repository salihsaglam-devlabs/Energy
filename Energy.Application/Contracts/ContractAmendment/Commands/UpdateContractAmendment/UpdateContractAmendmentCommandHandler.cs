using Energy.Application.Contracts.ContractAmendment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractAmendment.Commands.UpdateContractAmendment;

/// <summary>
/// <see cref="UpdateContractAmendmentCommand"/> handler'ı. <see cref="IContractAmendmentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateContractAmendmentCommandHandler
    : IRequestHandler<UpdateContractAmendmentCommand, BaseResponse<bool>>
{
    private readonly IContractAmendmentService _service;

    public UpdateContractAmendmentCommandHandler(IContractAmendmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateContractAmendmentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
