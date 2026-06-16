using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Requests;
using Energy.Shared.Models.V1.Finance.Payment.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payment.Queries.GetPaymentList;

/// <summary>Sayfalanmış Payment listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetPaymentListQuery(GetPaymentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<PaymentListResponse>>>;
