using Energy.Application.Contracts.Contract.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;
using MediatR;

namespace Energy.Application.Contracts.Contract.Queries.GetContractById;

/// <summary>
/// <see cref="GetContractByIdQuery"/> handler'ı. <see cref="IContractService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractByIdQueryHandler
    : IRequestHandler<GetContractByIdQuery, BaseResponse<ContractDetailResponse>>
{
    private readonly IContractService _service;

    public GetContractByIdQueryHandler(IContractService service)
        => _service = service;

    public Task<BaseResponse<ContractDetailResponse>> Handle(
        GetContractByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
