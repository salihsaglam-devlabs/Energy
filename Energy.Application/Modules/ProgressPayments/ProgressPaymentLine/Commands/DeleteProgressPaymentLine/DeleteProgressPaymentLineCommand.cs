using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Commands.DeleteProgressPaymentLine;

/// <summary>ProgressPaymentLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteProgressPaymentLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
