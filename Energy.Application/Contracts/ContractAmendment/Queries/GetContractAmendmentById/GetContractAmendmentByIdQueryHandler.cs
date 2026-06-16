using Energy.Application.Contracts.ContractAmendment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractAmendment.Queries.GetContractAmendmentById;

/// <summary>
/// <see cref="GetContractAmendmentByIdQuery"/> handler'ı. <see cref="IContractAmendmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractAmendmentByIdQueryHandler
    : IRequestHandler<GetContractAmendmentByIdQuery, BaseResponse<ContractAmendmentDetailResponse>>
{
    private readonly IContractAmendmentService _service;

    public GetContractAmendmentByIdQueryHandler(IContractAmendmentService service)
        => _service = service;

    public Task<BaseResponse<ContractAmendmentDetailResponse>> Handle(
        GetContractAmendmentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
