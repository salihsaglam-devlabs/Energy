using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Queries.GetProgressPaymentDeductionById;

/// <summary>Kimliğe göre ProgressPaymentDeduction detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProgressPaymentDeductionByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProgressPaymentDeductionDetailResponse>>;
