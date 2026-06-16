using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPayment.Commands.DeleteProgressPayment;

/// <summary>ProgressPayment kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteProgressPaymentCommand(Guid Id) : IRequest<BaseResponse<bool>>;
