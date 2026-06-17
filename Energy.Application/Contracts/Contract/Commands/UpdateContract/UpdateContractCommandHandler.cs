using Energy.Application.Contracts.Contract.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Contracts.Contract.Commands.UpdateContract;

/// <summary>
/// <see cref="UpdateContractCommand"/> handler'ı. <see cref="IContractService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateContractCommandHandler
    : IRequestHandler<UpdateContractCommand, BaseResponse<bool>>
{
    private readonly IContractService _service;

    public UpdateContractCommandHandler(IContractService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateContractCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
