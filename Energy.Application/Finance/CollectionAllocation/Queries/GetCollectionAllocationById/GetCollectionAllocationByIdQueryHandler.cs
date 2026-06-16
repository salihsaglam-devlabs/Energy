using Energy.Application.Finance.CollectionAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.CollectionAllocation.Queries.GetCollectionAllocationById;

/// <summary>
/// <see cref="GetCollectionAllocationByIdQuery"/> handler'ı. <see cref="ICollectionAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetCollectionAllocationByIdQueryHandler
    : IRequestHandler<GetCollectionAllocationByIdQuery, BaseResponse<CollectionAllocationDetailResponse>>
{
    private readonly ICollectionAllocationService _service;

    public GetCollectionAllocationByIdQueryHandler(ICollectionAllocationService service)
        => _service = service;

    public Task<BaseResponse<CollectionAllocationDetailResponse>> Handle(
        GetCollectionAllocationByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
