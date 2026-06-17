using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Requests;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPayment.Commands.UpdateProgressPayment;

/// <summary>Var olan ProgressPayment kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProgressPaymentCommand(Guid Id, UpdateProgressPaymentRequest Request)
    : IRequest<BaseResponse<bool>>;
