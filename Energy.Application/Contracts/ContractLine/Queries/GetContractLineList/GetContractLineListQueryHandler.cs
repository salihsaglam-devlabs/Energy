using Energy.Application.Contracts.ContractLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractLine.Queries.GetContractLineList;

/// <summary>
/// <see cref="GetContractLineListQuery"/> handler'ı. <see cref="IContractLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractLineListQueryHandler
    : IRequestHandler<GetContractLineListQuery, BaseResponse<PaginatedResponse<ContractLineListResponse>>>
{
    private readonly IContractLineService _service;

    public GetContractLineListQueryHandler(IContractLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ContractLineListResponse>>> Handle(
        GetContractLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
