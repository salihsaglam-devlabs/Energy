using Energy.Application.Modules.Contracts.ContractLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractLine.Commands.UpdateContractLine;

/// <summary>
/// <see cref="UpdateContractLineCommand"/> handler'ı. <see cref="IContractLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateContractLineCommandHandler
    : IRequestHandler<UpdateContractLineCommand, BaseResponse<bool>>
{
    private readonly IContractLineService _service;

    public UpdateContractLineCommandHandler(IContractLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateContractLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
