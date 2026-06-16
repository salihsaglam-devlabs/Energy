using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPayment.Queries.GetProgressPaymentList;

/// <summary>Sayfalanmış ProgressPayment listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProgressPaymentListQuery(GetProgressPaymentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProgressPaymentListResponse>>>;
