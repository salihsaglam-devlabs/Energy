using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Responses;
using Energy.Application.Modules.Finance.Reports.PayableAging.Services;
using MediatR;

namespace Energy.Application.Modules.Finance.Reports.PayableAging.Queries.GetPayableAgingData;

/// <summary><see cref="GetPayableAgingDataQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetPayableAgingDataQueryHandler
    : IRequestHandler<GetPayableAgingDataQuery, BaseResponse<PaginatedResponse<PayableAgingRowResponse>>>
{
    private readonly IPayableAgingService _service;

    public GetPayableAgingDataQueryHandler(IPayableAgingService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<PaginatedResponse<PayableAgingRowResponse>>> Handle(GetPayableAgingDataQuery request, CancellationToken ct)
    {
        return await _service.GetDataAsync(request.Request, ct);
    }
}
