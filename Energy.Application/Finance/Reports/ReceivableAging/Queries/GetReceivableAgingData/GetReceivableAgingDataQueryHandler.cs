using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Responses;
using Energy.Application.Finance.Reports.ReceivableAging.Services;
using MediatR;

namespace Energy.Application.Finance.Reports.ReceivableAging.Queries.GetReceivableAgingData;

/// <summary><see cref="GetReceivableAgingDataQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetReceivableAgingDataQueryHandler
    : IRequestHandler<GetReceivableAgingDataQuery, BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>>
{
    private readonly IReceivableAgingService _service;

    public GetReceivableAgingDataQueryHandler(IReceivableAgingService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>> Handle(GetReceivableAgingDataQuery request, CancellationToken ct)
    {
        return await _service.GetDataAsync(request.Request, ct);
    }
}
