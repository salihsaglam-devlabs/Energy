using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Requests;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPaymentLine.Commands.UpdateProgressPaymentLine;

/// <summary>Var olan ProgressPaymentLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProgressPaymentLineCommand(Guid Id, UpdateProgressPaymentLineRequest Request)
    : IRequest<BaseResponse<bool>>;
