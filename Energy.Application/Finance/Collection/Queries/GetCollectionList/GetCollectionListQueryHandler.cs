using Energy.Application.Finance.Collection.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Responses;
using MediatR;

namespace Energy.Application.Finance.Collection.Queries.GetCollectionList;

/// <summary>
/// <see cref="GetCollectionListQuery"/> handler'ı. <see cref="ICollectionService"/>'i orkestre eder.
/// </summary>
public sealed class GetCollectionListQueryHandler
    : IRequestHandler<GetCollectionListQuery, BaseResponse<PaginatedResponse<CollectionListResponse>>>
{
    private readonly ICollectionService _service;

    public GetCollectionListQueryHandler(ICollectionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<CollectionListResponse>>> Handle(
        GetCollectionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
