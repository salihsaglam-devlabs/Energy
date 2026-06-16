using Energy.Application.Core.UnitOfMeasure.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;
using MediatR;

namespace Energy.Application.Core.UnitOfMeasure.Queries.GetUnitOfMeasureById;

/// <summary>
/// <see cref="GetUnitOfMeasureByIdQuery"/> handler'ı. <see cref="IUnitOfMeasureService"/>'i orkestre eder.
/// </summary>
public sealed class GetUnitOfMeasureByIdQueryHandler
    : IRequestHandler<GetUnitOfMeasureByIdQuery, BaseResponse<UnitOfMeasureDetailResponse>>
{
    private readonly IUnitOfMeasureService _service;

    public GetUnitOfMeasureByIdQueryHandler(IUnitOfMeasureService service)
        => _service = service;

    public Task<BaseResponse<UnitOfMeasureDetailResponse>> Handle(
        GetUnitOfMeasureByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
