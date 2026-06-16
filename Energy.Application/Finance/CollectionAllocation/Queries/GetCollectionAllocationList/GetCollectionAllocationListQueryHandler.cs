using Energy.Application.Finance.CollectionAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.CollectionAllocation.Queries.GetCollectionAllocationList;

/// <summary>
/// <see cref="GetCollectionAllocationListQuery"/> handler'ı. <see cref="ICollectionAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetCollectionAllocationListQueryHandler
    : IRequestHandler<GetCollectionAllocationListQuery, BaseResponse<PaginatedResponse<CollectionAllocationListResponse>>>
{
    private readonly ICollectionAllocationService _service;

    public GetCollectionAllocationListQueryHandler(ICollectionAllocationService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<CollectionAllocationListResponse>>> Handle(
        GetCollectionAllocationListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
