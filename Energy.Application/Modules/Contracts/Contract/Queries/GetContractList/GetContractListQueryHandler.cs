using Energy.Application.Modules.Contracts.Contract.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.Contract.Queries.GetContractList;

/// <summary>
/// <see cref="GetContractListQuery"/> handler'ı. <see cref="IContractService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractListQueryHandler
    : IRequestHandler<GetContractListQuery, BaseResponse<PaginatedResponse<ContractListResponse>>>
{
    private readonly IContractService _service;

    public GetContractListQueryHandler(IContractService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ContractListResponse>>> Handle(
        GetContractListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
