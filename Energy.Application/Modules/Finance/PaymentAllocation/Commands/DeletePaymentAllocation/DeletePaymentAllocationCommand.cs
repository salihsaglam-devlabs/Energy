using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.PaymentAllocation.Commands.DeletePaymentAllocation;

/// <summary>PaymentAllocation kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeletePaymentAllocationCommand(Guid Id) : IRequest<BaseResponse<bool>>;
