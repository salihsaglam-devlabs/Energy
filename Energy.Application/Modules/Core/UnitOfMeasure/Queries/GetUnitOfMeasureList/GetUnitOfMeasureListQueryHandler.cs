using Energy.Application.Modules.Core.UnitOfMeasure.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitOfMeasure.Queries.GetUnitOfMeasureList;

/// <summary>
/// <see cref="GetUnitOfMeasureListQuery"/> handler'ı. <see cref="IUnitOfMeasureService"/>'i orkestre eder.
/// </summary>
public sealed class GetUnitOfMeasureListQueryHandler
    : IRequestHandler<GetUnitOfMeasureListQuery, BaseResponse<PaginatedResponse<UnitOfMeasureListResponse>>>
{
    private readonly IUnitOfMeasureService _service;

    public GetUnitOfMeasureListQueryHandler(IUnitOfMeasureService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<UnitOfMeasureListResponse>>> Handle(
        GetUnitOfMeasureListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
