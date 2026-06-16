using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Commands.UpdateProgressPaymentDeduction;

/// <summary>Var olan ProgressPaymentDeduction kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProgressPaymentDeductionCommand(Guid Id, UpdateProgressPaymentDeductionRequest Request)
    : IRequest<BaseResponse<bool>>;
