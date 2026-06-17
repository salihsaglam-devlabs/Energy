using Energy.Application.Core.UnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;
using MediatR;

namespace Energy.Application.Core.UnitConversion.Queries.GetUnitConversionList;

/// <summary>
/// <see cref="GetUnitConversionListQuery"/> handler'ı. <see cref="IUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class GetUnitConversionListQueryHandler
    : IRequestHandler<GetUnitConversionListQuery, BaseResponse<PaginatedResponse<UnitConversionListResponse>>>
{
    private readonly IUnitConversionService _service;

    public GetUnitConversionListQueryHandler(IUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<UnitConversionListResponse>>> Handle(
        GetUnitConversionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
