using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.PaymentAllocation.Queries.GetPaymentAllocationById;

/// <summary>Kimliğe göre PaymentAllocation detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetPaymentAllocationByIdQuery(Guid Id)
    : IRequest<BaseResponse<PaymentAllocationDetailResponse>>;
