using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payment.Queries.GetPaymentById;

/// <summary>Kimliğe göre Payment detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetPaymentByIdQuery(Guid Id)
    : IRequest<BaseResponse<PaymentDetailResponse>>;
