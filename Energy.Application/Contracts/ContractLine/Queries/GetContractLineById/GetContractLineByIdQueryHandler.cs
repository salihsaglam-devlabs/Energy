using Energy.Application.Contracts.ContractLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractLine.Queries.GetContractLineById;

/// <summary>
/// <see cref="GetContractLineByIdQuery"/> handler'ı. <see cref="IContractLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractLineByIdQueryHandler
    : IRequestHandler<GetContractLineByIdQuery, BaseResponse<ContractLineDetailResponse>>
{
    private readonly IContractLineService _service;

    public GetContractLineByIdQueryHandler(IContractLineService service)
        => _service = service;

    public Task<BaseResponse<ContractLineDetailResponse>> Handle(
        GetContractLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
