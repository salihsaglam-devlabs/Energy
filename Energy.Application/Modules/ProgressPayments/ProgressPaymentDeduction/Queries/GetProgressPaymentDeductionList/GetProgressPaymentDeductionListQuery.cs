using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Queries.GetProgressPaymentDeductionList;

/// <summary>Sayfalanmış ProgressPaymentDeduction listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProgressPaymentDeductionListQuery(GetProgressPaymentDeductionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>>;
