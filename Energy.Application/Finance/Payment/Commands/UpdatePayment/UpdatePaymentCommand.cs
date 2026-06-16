using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Requests;
using MediatR;

namespace Energy.Application.Finance.Payment.Commands.UpdatePayment;

/// <summary>Var olan Payment kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdatePaymentCommand(Guid Id, UpdatePaymentRequest Request)
    : IRequest<BaseResponse<bool>>;
