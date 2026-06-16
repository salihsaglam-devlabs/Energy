using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPayment.Queries.GetProgressPaymentById;

/// <summary>Kimliğe göre ProgressPayment detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProgressPaymentByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProgressPaymentDetailResponse>>;
