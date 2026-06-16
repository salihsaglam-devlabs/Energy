using Energy.Application.Modules.Contracts.ContractAmendment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractAmendment.Queries.GetContractAmendmentList;

/// <summary>
/// <see cref="GetContractAmendmentListQuery"/> handler'ı. <see cref="IContractAmendmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractAmendmentListQueryHandler
    : IRequestHandler<GetContractAmendmentListQuery, BaseResponse<PaginatedResponse<ContractAmendmentListResponse>>>
{
    private readonly IContractAmendmentService _service;

    public GetContractAmendmentListQueryHandler(IContractAmendmentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ContractAmendmentListResponse>>> Handle(
        GetContractAmendmentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
