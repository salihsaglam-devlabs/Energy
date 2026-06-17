using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.PaymentAllocation.Queries.GetPaymentAllocationList;

/// <summary>Sayfalanmış PaymentAllocation listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetPaymentAllocationListQuery(GetPaymentAllocationListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<PaymentAllocationListResponse>>>;
