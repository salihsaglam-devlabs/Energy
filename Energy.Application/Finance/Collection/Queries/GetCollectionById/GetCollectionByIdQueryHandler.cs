using Energy.Application.Finance.Collection.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Responses;
using MediatR;

namespace Energy.Application.Finance.Collection.Queries.GetCollectionById;

/// <summary>
/// <see cref="GetCollectionByIdQuery"/> handler'ı. <see cref="ICollectionService"/>'i orkestre eder.
/// </summary>
public sealed class GetCollectionByIdQueryHandler
    : IRequestHandler<GetCollectionByIdQuery, BaseResponse<CollectionDetailResponse>>
{
    private readonly ICollectionService _service;

    public GetCollectionByIdQueryHandler(ICollectionService service)
        => _service = service;

    public Task<BaseResponse<CollectionDetailResponse>> Handle(
        GetCollectionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
