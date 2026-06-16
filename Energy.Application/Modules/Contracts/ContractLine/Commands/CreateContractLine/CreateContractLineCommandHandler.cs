using Energy.Application.Modules.Contracts.ContractLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractLine.Commands.CreateContractLine;

/// <summary>
/// <see cref="CreateContractLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IContractLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateContractLineCommandHandler
    : IRequestHandler<CreateContractLineCommand, BaseResponse<Guid>>
{
    private readonly IContractLineService _service;

    public CreateContractLineCommandHandler(IContractLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateContractLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
