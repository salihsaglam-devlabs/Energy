using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Requests;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.Reports.ProgressPaymentSummary.Queries.GetProgressPaymentSummaryData;

/// <summary>ProgressPaymentSummary rapor verisi (filtreli, sayfalı).</summary>
public sealed record GetProgressPaymentSummaryDataQuery(ProgressPaymentSummaryRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>>;
