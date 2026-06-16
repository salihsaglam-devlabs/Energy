using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Commands.DeleteProgressPaymentDeduction;

/// <summary>ProgressPaymentDeduction kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteProgressPaymentDeductionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
