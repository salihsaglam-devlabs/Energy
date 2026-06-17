using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPaymentLine.Queries.GetProgressPaymentLineList;

/// <summary>Sayfalanmış ProgressPaymentLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProgressPaymentLineListQuery(GetProgressPaymentLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProgressPaymentLineListResponse>>>;
