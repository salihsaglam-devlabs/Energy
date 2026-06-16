using Energy.Application.Modules.Core.UnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitConversion.Queries.GetUnitConversionById;

/// <summary>
/// <see cref="GetUnitConversionByIdQuery"/> handler'ı. <see cref="IUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class GetUnitConversionByIdQueryHandler
    : IRequestHandler<GetUnitConversionByIdQuery, BaseResponse<UnitConversionDetailResponse>>
{
    private readonly IUnitConversionService _service;

    public GetUnitConversionByIdQueryHandler(IUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<UnitConversionDetailResponse>> Handle(
        GetUnitConversionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
