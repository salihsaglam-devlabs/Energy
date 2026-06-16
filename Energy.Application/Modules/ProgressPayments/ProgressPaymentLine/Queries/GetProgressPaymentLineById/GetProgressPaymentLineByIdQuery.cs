using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Queries.GetProgressPaymentLineById;

/// <summary>Kimliğe göre ProgressPaymentLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProgressPaymentLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProgressPaymentLineDetailResponse>>;
