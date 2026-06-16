using Energy.Application.Contracts.ContractAmendment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractAmendment.Commands.CreateContractAmendment;

/// <summary>
/// <see cref="CreateContractAmendmentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IContractAmendmentService"/>'i orkestre eder.
/// </summary>
public sealed class CreateContractAmendmentCommandHandler
    : IRequestHandler<CreateContractAmendmentCommand, BaseResponse<Guid>>
{
    private readonly IContractAmendmentService _service;

    public CreateContractAmendmentCommandHandler(IContractAmendmentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateContractAmendmentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
