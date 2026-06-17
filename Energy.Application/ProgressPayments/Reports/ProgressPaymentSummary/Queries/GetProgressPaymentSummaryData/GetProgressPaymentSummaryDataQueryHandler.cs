using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Requests;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Responses;
using Energy.Application.ProgressPayments.Reports.ProgressPaymentSummary.Services;
using MediatR;

namespace Energy.Application.ProgressPayments.Reports.ProgressPaymentSummary.Queries.GetProgressPaymentSummaryData;

/// <summary><see cref="GetProgressPaymentSummaryDataQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetProgressPaymentSummaryDataQueryHandler
    : IRequestHandler<GetProgressPaymentSummaryDataQuery, BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>>
{
    private readonly IProgressPaymentSummaryService _service;

    public GetProgressPaymentSummaryDataQueryHandler(IProgressPaymentSummaryService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>> Handle(GetProgressPaymentSummaryDataQuery request, CancellationToken ct)
    {
        return await _service.GetDataAsync(request.Request, ct);
    }
}
